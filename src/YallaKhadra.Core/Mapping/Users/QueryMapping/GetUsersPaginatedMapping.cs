using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void GetUsersPaginatedMapping() {
            CreateMap<DomainUser, GetUsersPaginatedResponse>()
                .IncludeBase<DomainUser, UserResponse>()
                .ForMember(dest => dest.Phone,
                   opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}


