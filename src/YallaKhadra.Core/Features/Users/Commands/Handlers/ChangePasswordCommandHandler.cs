using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Services;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Features.Users.Commands.Handlers {

    public class ChangePasswordCommandHandler : ResponseHandler, IRequestHandler<ChangePasswordCommand, Response> {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUserService _currentUserService;


        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, ICurrentUserService currentUserService) {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _currentUserService = currentUserService;
        }

        public async Task<Response> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;
            var changeResult = await _authenticationService.ChangePassword(userId!.Value, request.CurrentPassword, request.NewPassword);
            if (changeResult.Succeeded)
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            return FromServiceResult(changeResult);

        }
    }
}
