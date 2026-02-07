using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Services {
    public interface IApplicationUserService {
        public Task<ServiceOperationResult<DomainUser>> AddUser(DomainUser user, string password, UserRole? role = null);
        //public Task<bool> SendConfirmationEmailAsync(ApplicationUser user);
        //public Task<ServiceOperationResult<string?>> ConfirmEmailAsync(int userId, string code);
        //public Task<ServiceOperationResult<string?>> ResetPasswordAsync(ApplicationUser user, string newPassword);

    }
}
