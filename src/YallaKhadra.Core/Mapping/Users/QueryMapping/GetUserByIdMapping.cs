using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void GetUserByIdMapping() {
            CreateMap<DomainUser, GetUserByIdResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }
    }
}