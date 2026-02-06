using MediatR;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Features.Users.Queries.Models {
    public class CheckEmailAvailabilityQuery : IRequest<Response<CheckEmailAvailabilityResponse>> {
        public string Email { get; set; }
    }
}
