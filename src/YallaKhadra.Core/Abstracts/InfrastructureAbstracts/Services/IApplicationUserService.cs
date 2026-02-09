using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories {
    public interface IApplicationUserService {
        public Task<ServiceOperationResult<DomainUser>> AddUser(DomainUser user, string password, UserRole? role = null, CancellationToken ct = default);


    }

}