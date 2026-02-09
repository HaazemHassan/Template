using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;
using YallaKhadra.Core.Features.Users.Commands.Responses;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void AddUserMapping() {
            CreateMap<AddUserCommand, DomainUser>();
            CreateMap<DomainUser, AddUserResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }
    }
}
