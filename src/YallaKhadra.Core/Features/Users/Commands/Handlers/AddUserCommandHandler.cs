using AutoMapper;
using MediatR;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;
using YallaKhadra.Core.Features.Users.Commands.Responses;

namespace YallaKhadra.Core.Features.Users.Commands.Handlers {
    public class AddUserCommandHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<AddUserResponse>> {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;

        public AddUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
        }

        public async Task<Response<AddUserResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken) {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            var domainUser = _mapper.Map<DomainUser>(request);
            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, request.UserRole, cancellationToken);

            if (!addUserResult.Succeeded)
                return FromServiceResult<AddUserResponse>(addUserResult);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<AddUserResponse>(addUserResult.Data);
            await transaction.CommitAsync(cancellationToken);
            return Created(response);
        }
    }
}
