using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesContracts;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.IdentityEntities;
using YallaKhadra.Core.Features.Authentication.Commands.RequestsModels;

namespace YallaKhadra.Core.Features.Authentication.Commands.Handlers;

public class AuthenticationCommandHandler : ResponseHandler,
                                                         IRequestHandler<SignInCommand, Response<AuthResult>>,
                                                         IRequestHandler<RefreshTokenCommand, Response<AuthResult>>,
                                                         IRequestHandler<LogoutCommand, Response> {


    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;


    public AuthenticationCommandHandler(IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager, IMapper mapper, IAuthenticationService authenticationService, ICurrentUserService currentUserService, IUnitOfWork unitOfWork) {
        _applicationUserService = applicationUserService;
        _userManager = userManager;
        _mapper = mapper;
        _authenticationService = authenticationService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }



    public async Task<Response<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.SignInWithPassword(request.Email, request.Password);
        if (authResult.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }

    public async Task<Response<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.ReAuthenticateAsync(request.RefreshToken!, request.AccessToken);
        if (authResult.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }

    public async Task<Response> Handle(LogoutCommand request, CancellationToken cancellationToken) {
        var serviceResult = await _authenticationService.LogoutAsync(request.RefreshToken!);
        if (serviceResult.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(serviceResult);
    }
}
