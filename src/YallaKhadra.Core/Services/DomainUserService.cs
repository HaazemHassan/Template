using YallaKhadra.Core.Abstracts.CoreAbstracts.Services;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Services {
    public class DomainUserService : IDomainUserService {
        private readonly IUnitOfWork _unitOfWork;

        public DomainUserService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceOperationResult<DomainUser>> UpdateProfile(DomainUser user, CancellationToken cancellationToken = default) {
            if (user is null)
                return ServiceOperationResult<DomainUser>.Failure(ServiceOperationStatus.InvalidParameters, "User cannot be null");

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            return ServiceOperationResult<DomainUser>.Success(user, ServiceOperationStatus.Updated, "User profile updated successfully");


        }
    }
}
