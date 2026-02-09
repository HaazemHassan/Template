using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users;
using YallaKhadra.Core.Features.Users.Commands.Responses;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void UpdateUserProfileMapping() {
            CreateMap<DomainUser, UpdateProfileResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }
    }
}
