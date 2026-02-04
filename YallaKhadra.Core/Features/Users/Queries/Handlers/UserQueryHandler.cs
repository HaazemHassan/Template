using AutoMapper;
using MediatR;
using YallaKhadra.Core.Abstracts.ApiAbstracts;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases.Pagination;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Users.Queries.Models;
using YallaKhadra.Core.Features.Users.Queries.Responses;
using YallaKhadra.Core.Features.Users.Queries.Specefications;

namespace YallaKhadra.Core.Features.Users.Queries.Handlers {
public class UserQueryHandler : ResponseHandler,
                                IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedResponse>>,
                                IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>,
                                IRequestHandler<CheckEmailAvailabilityQuery, Response<CheckEmailAvailabilityResponse>> {
    private readonly IMapper _mapper;
    private readonly IApplicationUserService _applicationUserService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public UserQueryHandler(IMapper mapper, IApplicationUserService applicationUserService, ICurrentUserService currentUserService, IUnitOfWork unitOfWork) {
        _mapper = mapper;
        _applicationUserService = applicationUserService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }


    public async Task<PaginatedResult<GetUsersPaginatedResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken) {
        var dataSpec = new UsersFilterPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.SortBy);
        var countSpec = new UsersFilterCountSpec(request.Search);

        var items = await _unitOfWork.Users.ListAsync(dataSpec, cancellationToken);
        var totalCount = await _unitOfWork.Users.CountAsync(countSpec, cancellationToken);

        return PaginatedResult<GetUsersPaginatedResponse>.Success(items, totalCount, request.PageNumber, request.PageSize);
    }

    public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user is null)
            return NotFound<GetUserByIdResponse>();

        var userResponse = _mapper.Map<GetUserByIdResponse>(user);
        return Success(userResponse);
    }


    public async Task<Response<CheckEmailAvailabilityResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken) {
        var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email);
        var response = new CheckEmailAvailabilityResponse {
            IsAvailable = user is null
        };
        return Success(response, message: response.IsAvailable ? "Email is available." : "Email is not available.");

    }
}
}
