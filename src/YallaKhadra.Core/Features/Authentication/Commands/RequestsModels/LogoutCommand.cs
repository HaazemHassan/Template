using MediatR;
using YallaKhadra.Core.Bases.Responses;

namespace YallaKhadra.Core.Features.Authentication.Commands.RequestsModels {
    public class LogoutCommand : IRequest<Response> {
        public string? RefreshToken { get; set; }
    }
}
