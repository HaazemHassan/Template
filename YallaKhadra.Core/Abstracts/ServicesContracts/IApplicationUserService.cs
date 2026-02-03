using YallaKhadra.Core.Bases;
using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Abstracts.ServicesContracts {
    public interface IApplicationUserService {
        public Task<ServiceOperationResult<User>> AddUser(User user, string password, UserRole role = UserRole.User);
        //public Task<bool> SendConfirmationEmailAsync(ApplicationUser user);
        //public Task<ServiceOperationResult<string?>> ConfirmEmailAsync(int userId, string code);
        //public Task<ServiceOperationResult<string?>> ResetPasswordAsync(ApplicationUser user, string newPassword);

    }
}
