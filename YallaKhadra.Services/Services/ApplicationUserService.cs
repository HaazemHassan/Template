using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesContracts;
using YallaKhadra.Core.Bases;
using YallaKhadra.Core.Entities.IdentityEntities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Services.Services {
    public class ApplicationUserService : IApplicationUserService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientContextService _clientContextService;
        private readonly ICurrentUserService _currentUserService;


        public ApplicationUserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IClientContextService clientContextService, ICurrentUserService currentUserService) {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _clientContextService = clientContextService;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceOperationResult<ApplicationUser>> AddUser(ApplicationUser user, string password, UserRole role = UserRole.User) {
            if (_currentUserService.IsAuthenticated && !_currentUserService.IsInRole(UserRole.Admin))
                return ServiceOperationResult<ApplicationUser>.Failure(ServiceOperationStatus.Forbidden);
            else if (!_currentUserService.IsAuthenticated)
                role = UserRole.User;



            user.UserName = user.Email;

            if (await _userManager.Users.AnyAsync(x => x.Email == user.Email))
                return ServiceOperationResult<ApplicationUser>.
                    Failure(ServiceOperationStatus.AlreadyExists, "Email already exists.");


            if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
                return ServiceOperationResult<ApplicationUser>.
                    Failure(ServiceOperationStatus.AlreadyExists, "This phone number is used.");

            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
                return ServiceOperationResult<ApplicationUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role.ToString());
            if (!addToRoleResult.Succeeded)
                return ServiceOperationResult<ApplicationUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");


            //var succedded = await SendConfirmationEmailAsync(user);
            //if (!succedded)
            //    return ServiceOperationResult.Failed;

            return ServiceOperationResult<ApplicationUser>.
                Success(user, ServiceOperationStatus.Created);
        }
    }
}
