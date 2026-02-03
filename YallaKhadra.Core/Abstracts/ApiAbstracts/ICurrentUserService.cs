using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Enums;

namespace YallaKhadra.Core.Abstracts.ApiAbstracts;

public interface ICurrentUserService {
    int? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    Task<User?> GetCurrentUserAsync();
    IList<UserRole> GetRoles();
    bool IsInRole(UserRole roleName);
}