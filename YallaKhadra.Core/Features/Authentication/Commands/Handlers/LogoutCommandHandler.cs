using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Authentication.Commands.RequestsModels;

namespace YallaKhadra.Core.Features.Authentication.Commands.Handlers;

public class LogoutCommandHandler : ResponseHandler, IRequestHandler<LogoutCommand, Response> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(LogoutCommand request, CancellationToken cancellationToken) {
        var serviceResult = await _authenticationService.LogoutAsync(request.RefreshToken!);
        if (serviceResult.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(serviceResult);
    }
}
