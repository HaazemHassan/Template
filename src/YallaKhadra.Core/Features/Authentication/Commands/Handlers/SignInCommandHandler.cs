using MediatR;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Authentication.Commands.RequestsModels;

namespace YallaKhadra.Core.Features.Authentication.Commands.Handlers;

public class SignInCommandHandler : ResponseHandler, IRequestHandler<SignInCommand, Response<AuthResult>> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public SignInCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.SignInWithPassword(request.Email, request.Password);
        if (authResult.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }
}
