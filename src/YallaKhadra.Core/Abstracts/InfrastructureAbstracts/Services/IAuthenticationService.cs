using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Bases.Responses;

namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Services {
    public interface IAuthenticationService {
        public Task<ServiceOperationResult<AuthResult>> SignInWithPassword(string Email, string passwod);
        public bool ValidateAccessToken(string token, bool validateLifetime = true);
        public Task<ServiceOperationResult<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken);
        public Task<ServiceOperationResult> LogoutAsync(string refreshToken);

    }
}
