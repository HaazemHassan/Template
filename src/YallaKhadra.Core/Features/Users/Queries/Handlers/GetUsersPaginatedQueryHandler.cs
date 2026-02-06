using MediatR;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Bases.Pagination;
using YallaKhadra.Core.Bases.Responses;
using YallaKhadra.Core.Features.Users.Queries.Models;
using YallaKhadra.Core.Features.Users.Queries.Responses;
using YallaKhadra.Core.Features.Users.Queries.Specefications;

namespace YallaKhadra.Core.Features.Users.Queries.Handlers {
    public class GetUsersPaginatedQueryHandler : ResponseHandler, IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedResponse>> {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersPaginatedQueryHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetUsersPaginatedResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken) {
            var dataSpec = new UsersFilterPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.SortBy);
            var countSpec = new UsersFilterCountSpec(request.Search);

            var items = await _unitOfWork.Users.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Users.CountAsync(countSpec, cancellationToken);

            return PaginatedResult<GetUsersPaginatedResponse>.Success(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
