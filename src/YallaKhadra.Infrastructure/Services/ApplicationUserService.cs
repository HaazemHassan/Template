using Microsoft.AspNetCore.Identity;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Services;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.IdentityEntities;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Infrastructure.Services {
    public class ApplicationUserService : IApplicationUserService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientContextService _clientContextService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;


        public ApplicationUserService(IUnitOfWork unitOfWork, IClientContextService clientContextService, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager) {
            _unitOfWork = unitOfWork;
            _clientContextService = clientContextService;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<ServiceOperationResult<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User) {
            if (_currentUserService.IsAuthenticated && !_currentUserService.IsInRole(UserRole.Admin))
                return ServiceOperationResult<DomainUser>.Failure(ServiceOperationStatus.Forbidden);
            else if (!_currentUserService.IsAuthenticated)
                role = UserRole.User;

            if (await _unitOfWork.Users.AnyAsync(x => x.Email == user.Email))
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.AlreadyExists, "Email already exists.");

            if (await _unitOfWork.Users.AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.AlreadyExists, "This phone number is used.");

            var applicationUser = new ApplicationUser {
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DomainUser = user
            };

            var createResult = await _userManager.CreateAsync(applicationUser, password);

            if (!createResult.Succeeded)
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");

            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, role.ToString());
            if (!addToRoleResult.Succeeded)
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");


            //var succedded = await SendConfirmationEmailAsync(applicationUser);
            //if (!succedded)
            //    return ServiceOperationResult.Failed;

            return ServiceOperationResult<DomainUser>.
                Success(user, ServiceOperationStatus.Created);
        }
    }
}
