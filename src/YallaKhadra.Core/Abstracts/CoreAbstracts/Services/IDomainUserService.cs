using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;

namespace YallaKhadra.Core.Abstracts.CoreAbstracts.Services {
    public interface IDomainUserService {

        public Task<ServiceOperationResult<DomainUser>> UpdateProfile(DomainUser user, CancellationToken cancellationToken = default);
    }
}
