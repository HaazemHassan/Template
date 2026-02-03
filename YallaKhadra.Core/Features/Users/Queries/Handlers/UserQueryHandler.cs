using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesContracts;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Users.Queries.Models;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Features.Users.Queries.Handlers {
    public class UserQueryHandler : ResponseHandler,
                                    IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedResponse>>,
                                    IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>,
                                    IRequestHandler<CheckEmailAvailabilityQuery, Response<CheckEmailAvailabilityResponse>> {
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public UserQueryHandler(IMapper mapper, IApplicationUserService applicationUserService, ICurrentUserService currentUserService, IUserRepository userRepository) {
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<PaginatedResult<GetUsersPaginatedResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken) {

            var usersQuerable = _userRepository.GetTableNoTracking()
                                                .ProjectTo<GetUsersPaginatedResponse>(_mapper.ConfigurationProvider);
            var usersPaginatedResult = await _userRepository.GetPaginatedListAsync(usersQuerable, request.PageNumber, request.PageSize);
            return usersPaginatedResult;

        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user is null)
                return NotFound<GetUserByIdResponse>();

            var userResponse = _mapper.Map<GetUserByIdResponse>(user);
            return Success(userResponse);
        }


        public async Task<Response<CheckEmailAvailabilityResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken) {
            var user = await _userRepository.GetAsync(u => u.Email == request.Email);
            var response = new CheckEmailAvailabilityResponse {
                IsAvailable = user is null
            };
            return Success(response, message: response.IsAvailable ? "Email is available." : "Email is not available.");

        }
    }
}
