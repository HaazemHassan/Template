using AutoMapper;
using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;
using YallaKhadra.Core.Features.Users.Commands.Responses;

namespace YallaKhadra.Core.Features.Users.Commands.Handlers {
    public class UserCommandHanlder : ResponseHandler, IRequestHandler<RegisterCommand, Response<AuthResult>>, IRequestHandler<AddUserCommand, Response<AddUserResponse>> {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthenticationService _authenticationService;

        public UserCommandHanlder(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, ICurrentUserService currentUserService, IAuthenticationService authenticationService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
            _authenticationService = authenticationService;
        }


        public async Task<Response<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var userMapped = _mapper.Map<User>(request);
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


        public async Task<Response<AddUserResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken) {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var domainUser = _mapper.Map<User>(request);
            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, request.UserRole!.Value);


            if (!addUserResult.IsSuccess || addUserResult.Data is null)
                return FromServiceResult<AddUserResponse>(addUserResult);

            var response = _mapper.Map<AddUserResponse>(addUserResult.Data);
            await transaction.CommitAsync(cancellationToken);
            return Created(response);
        }

    }
}
