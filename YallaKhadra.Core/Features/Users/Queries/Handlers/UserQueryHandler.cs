using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.ServicesContracts;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Entities.IdentityEntities;
using YallaKhadra.Core.Features.Users.Queries.Models;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Features.Users.Queries.Handlers {
    public class UserQueryHandler : ResponseHandler,
                                    IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedResponse>>,
                                    IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>,
                                    IRequestHandler<CheckEmailAvailabilityQuery, Response<CheckEmailAvailabilityResponse>> {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICurrentUserService _currentUserService;

        public UserQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, IApplicationUserService applicationUserService, ICurrentUserService currentUserService) {
            _userManager = userManager;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetUsersPaginatedResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken) {

            var usersQuerable = _userManager.Users;
            var usersPaginatedResult = await usersQuerable.ProjectTo<GetUsersPaginatedResponse>(_mapper.ConfigurationProvider)
                                                          .ToPaginatedResultAsync(request.PageNumber, request.PageSize);
            return usersPaginatedResult;

        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user is null)
                return NotFound<GetUserByIdResponse>();

            var userResponse = _mapper.Map<GetUserByIdResponse>(user);
            return Success(userResponse);
        }


        public async Task<Response<CheckEmailAvailabilityResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken) {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var response = new CheckEmailAvailabilityResponse {
                IsAvailable = user is null
            };
            return Success(response, message: response.IsAvailable ? "Email is available." : "Email is not available.");

        }
    }
}
