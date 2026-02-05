using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void GetUserByIdMapping() {
            CreateMap<DomainUser, GetUserByIdResponse>();
        }
    }
}