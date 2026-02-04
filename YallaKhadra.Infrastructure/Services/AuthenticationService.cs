using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Entities.IdentityEntities;
using YallaKhadra.Core.Enums;
using YallaKhadra.Core.Features.Users.Queries.Responses;
using YallaKhadra.Infrastructure.Abstracts;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Services.Services {
    public class AuthenticationService : IAuthenticationService {

        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDbContext _dbContext;
        //private readonly IEmailService _emailService;



        public AuthenticationService(JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, IRefreshTokenRepository refreshTokenRepository, IApplicationUserService applicationUserService, IMapper mapper /*IEmailService emailService*/, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService, AppDbContext dbContext) {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            //_emailService = emailService;
        }

        public async Task<ServiceOperationResult<AuthResult>> SignInWithPassword(string email, string passwod) {
            var userFromDb = await _dbContext.Set<ApplicationUser>().Include(u => u.RefreshTokens).Include(u => u.DomainUser)
                                .FirstOrDefaultAsync(u => u.Email == email && u.DomainUser!.Email == email);
            if (userFromDb is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid Email or password");

            bool isAuthenticated = await _userManager.CheckPasswordAsync(userFromDb, passwod);
            if (!isAuthenticated)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid Email or password");

            //if (!userFromDb.EmailConfirmed)
            //return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Please confirm your email first");

            return await AuthenticateAsync(userFromDb);
        }

        public async Task<ServiceOperationResult<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken) {
            var isValidAccessToken = ValidateAccessToken(accessToken, validateLifetime: false);
            if (!isValidAccessToken)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid access token");

            var jwt = ReadJWT(accessToken);
            if (jwt is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Can't read this token");

            var domainUserId = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (domainUserId is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "User id is null");

            var appUser = await _dbContext.Set<ApplicationUser>().Include(au => au.DomainUser).FirstOrDefaultAsync(au => au.DomainUserId.ToString() == domainUserId);
            if (appUser is null || appUser.DomainUser is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "User is not found");

            var currentRefreshToken = await _refreshTokenRepository.GetAsync(x => x.AccessTokenJTI == jwt.Id &&
                                                                     x.Token == refreshToken &&
                                                                     x.UserId == appUser.Id);

            if (currentRefreshToken is null || !currentRefreshToken.IsActive)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Refresh token is not valid");

            //new jwt result
            var jwtResultOperation = await AuthenticateAsync(appUser, currentRefreshToken.Expires);
            if (!jwtResultOperation.IsSuccess || jwtResultOperation.Data is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Failed, "Failed to generate new token");

            currentRefreshToken.RevokationDate = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(currentRefreshToken);
            return jwtResultOperation;
        }



        public bool ValidateAccessToken(string token, bool validateLifetime = true) {
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(2)  //default = 5 min (security gap)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return principal is not null;
        }


        public async Task<ServiceOperationResult> LogoutAsync(string refreshToken) {
            if (!_currentUserService.IsAuthenticated)
                return ServiceOperationResult.Failure(ServiceOperationStatus.Unauthorized, "You're already signed out!");

            int domainUserId = _currentUserService.UserId!.Value;
            var appUser = await _userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId);
            if (appUser is null)
                return ServiceOperationResult.Failure(ServiceOperationStatus.NotFound, "User not found!");

            var refreshTokenFromDb = await _refreshTokenRepository.GetAsync(r => r.Token == refreshToken && r.UserId == appUser.Id);

            if (refreshTokenFromDb == null || !refreshTokenFromDb.IsActive)
                return ServiceOperationResult.Failure(ServiceOperationStatus.NotFound, "You maybe signed out!");

            refreshTokenFromDb.RevokationDate = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(refreshTokenFromDb);
            return ServiceOperationResult.Success(message: "Logout successfull");

        }

        #region Helper functions

        private async Task<ServiceOperationResult<AuthResult>> AuthenticateAsync(ApplicationUser appUser, DateTime? refreshTokenExpDate = null) {
            if (appUser is null || appUser.DomainUserId is null || appUser.DomainUser is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.InvalidParameters, "User cannot be null");

            var jwtSecurityToken = await GenerateAccessToken(appUser);
            var refreshToken = GenerateRefreshToken(appUser.Id, refreshTokenExpDate);
            await AddRefreshTokenToDatabase(refreshToken, jwtSecurityToken.Id);

            AuthResult jwtResult = new AuthResult {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                User = new GetUserByIdResponse {
                    Id = appUser.DomainUserId.Value,
                    Email = appUser.DomainUser.Email,
                    Address = appUser.DomainUser.Address!,
                    PhoneNumber = appUser.DomainUser.PhoneNumber!
                }
            };
            return ServiceOperationResult<AuthResult>.Success(jwtResult, message: "Logged in successfully");
        }

        private async Task<JwtSecurityToken> GenerateAccessToken(ApplicationUser user, List<Claim>? claims = null, DateTime? expDate = null) {
            return new JwtSecurityToken(
                  issuer: _jwtSettings.Issuer,
                  audience: _jwtSettings.Audience,
                  claims: claims ?? await GetUserClaims(user),
                  signingCredentials: GetSigningCredentials(),
                  expires: expDate ?? DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
              );
        }
        private async Task<List<Claim>> GetUserClaims(ApplicationUser user) {
            var claims = new List<Claim>()
             {
                 new Claim(ClaimTypes.NameIdentifier,user.DomainUserId!.Value.ToString()),
                 new Claim(ClaimTypes.Email,user.DomainUser!.Email),
                 new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

             };

            var customClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(customClaims);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles) {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }
        private SigningCredentials GetSigningCredentials() {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
        private RefreshTokenDTO GenerateRefreshToken(int userId, DateTime? expirationDate = null) {
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            string refreshTokenValue = Convert.ToBase64String(randomBytes);

            return new RefreshTokenDTO {
                Token = refreshTokenValue,
                ExpirationDate = expirationDate ?? DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };


        }
        private async Task AddRefreshTokenToDatabase(RefreshTokenDTO refreshTokenDTO, string accessTokenJti) {
            var refreshToken = new RefreshToken {
                Created = DateTime.UtcNow,
                Expires = refreshTokenDTO.ExpirationDate,
                AccessTokenJTI = accessTokenJti,
                Token = refreshTokenDTO.Token,
                UserId = refreshTokenDTO.UserId
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
        }
        private JwtSecurityToken ReadJWT(string accessToken) {
            if (string.IsNullOrEmpty(accessToken)) {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        private async Task<AuthResult?> GeneratePasswordResetToken(ApplicationUser user) {
            if (user is null)
                return null;

            var userClaims = await GetUserClaims(user);
            userClaims.Add(new Claim("purpose", "reset-password"));
            var jwtSecurityToken = await GenerateAccessToken(user, userClaims, DateTime.UtcNow.AddMinutes(5));

            AuthResult jwtResult = new AuthResult {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            };
            return jwtResult;
        }

        #endregion

    }
}