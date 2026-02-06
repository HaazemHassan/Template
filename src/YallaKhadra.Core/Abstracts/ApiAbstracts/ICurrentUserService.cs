using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Abstracts.ApiAbstracts;

public interface ICurrentUserService {
    int? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    Task<DomainUser?> GetCurrentUserAsync();
    IList<UserRole> GetRoles();
    bool IsInRole(UserRole roleName);
}