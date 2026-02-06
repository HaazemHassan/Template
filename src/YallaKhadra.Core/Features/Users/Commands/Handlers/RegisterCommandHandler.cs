using AutoMapper;
using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;

namespace YallaKhadra.Core.Features.Users.Commands.Handlers {
    public class RegisterCommandHandler : ResponseHandler, IRequestHandler<RegisterCommand, Response<AuthResult>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
        }

        public async Task<Response<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var userMapped = _mapper.Map<DomainUser>(request);
            var addUserResult = await _applicationUserService.AddUser(userMapped, request.Password);

            if (!addUserResult.IsSuccess || addUserResult.Data is null)
                return FromServiceResult<AuthResult>(addUserResult);

            var user = addUserResult.Data;

            var authResult = await _authenticationService.SignInWithPassword(user.Email, request.Password);
            if (!addUserResult.IsSuccess)
                FromServiceResult(authResult);

            await transaction.CommitAsync(cancellationToken);
            return FromServiceResult(authResult);
        }
    }
}
