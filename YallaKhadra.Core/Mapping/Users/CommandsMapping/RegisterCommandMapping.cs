using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Mapping.Users {
    public partial class UserProfile {
        public void RegisterMapping() {
            CreateMap<RegisterCommand, User>();

        }
    }
}
