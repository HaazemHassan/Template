using MediatR;
using YallaKhadra.Core.Attributes;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Users.Commands.Responses;

namespace YallaKhadra.Core.Features.Users.Commands.RequestModels {
    public class UpdateProfileCommand : IRequest<Response<UpdateProfileResponse>> {
        [SwaggerExclude]
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
