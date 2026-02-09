using AutoMapper;

namespace YallaKhadra.Core.Mapping.Users;

public partial class UserProfile : Profile {
    public UserProfile() {
        UserResponseMapping();  // Base mapping first
        RegisterMapping();
        AddUserMapping();
        GetUsersPaginatedMapping();
        GetUserByIdMapping();
        UpdateUserProfileMapping();
    }
}
