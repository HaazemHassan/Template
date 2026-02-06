using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void UpdateUserProfileMapping() {
            CreateMap<UpdateProfileCommand, DomainUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember)
                 => !string.IsNullOrWhiteSpace(srcMember as string)));
        }
    }
}
