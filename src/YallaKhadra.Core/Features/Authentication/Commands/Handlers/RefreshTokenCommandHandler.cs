using MediatR;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Services;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Authentication.Commands.RequestsModels;

namespace YallaKhadra.Core.Features.Authentication.Commands.Handlers;

public class RefreshTokenCommandHandler : ResponseHandler, IRequestHandler<RefreshTokenCommand, Response<AuthResult>> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var authResult = await _authenticationService.ReAuthenticateAsync(request.RefreshToken!, request.AccessToken);
        if (authResult.Succeeded)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }
}
