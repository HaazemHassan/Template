using YallaKhadra.Core.Bases;
using YallaKhadra.Core.Bases.Authentication;

namespace YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts {
    public interface IAuthenticationService {
        public Task<ServiceOperationResult<AuthResult>> SignInWithPassword(string Email, string passwod);
        public bool ValidateAccessToken(string token, bool validateLifetime = true);
        public Task<ServiceOperationResult<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken);
        public Task<ServiceOperationResult> LogoutAsync(string refreshToken);

    }
}
