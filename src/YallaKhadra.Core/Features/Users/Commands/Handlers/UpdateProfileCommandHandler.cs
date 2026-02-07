using AutoMapper;
using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.CoreAbstracts.Services;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Core.Features.Users.Commands.RequestModels;
using YallaKhadra.Core.Features.Users.Commands.Responses;

namespace YallaKhadra.Core.Features.Users.Commands.Handlers {
    public class UpdateProfileCommandHandler : ResponseHandler, IRequestHandler<UpdateProfileCommand, Response<UpdateProfileResponse>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDomainUserService _domainUserService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDomainUserService domainUserService, ICurrentUserService currentUserService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _domainUserService = domainUserService;
            _currentUserService = currentUserService;
        }

        public async Task<Response<UpdateProfileResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken) {
            var userToUpdate = _mapper.Map<UpdateProfileCommand, DomainUser>(request);
            var updateResult = await _domainUserService.UpdateProfile(userToUpdate, cancellationToken);

            if (!updateResult.IsSuccess)
                return FromServiceResult<UpdateProfileResponse>(updateResult);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userResponse = _mapper.Map<DomainUser, UpdateProfileResponse>(updateResult.Data);
            return FromServiceResult<UpdateProfileResponse>(updateResult);
        }

    }
}

