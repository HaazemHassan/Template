using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {

        public void UserResponseMapping() {
            CreateMap<DomainUser, UserResponse>()
                .ForMember(dest => dest.FullName,
                   opt => opt.MapFrom(src => src.FullName))
                .IncludeAllDerived();
        }
    }
}
