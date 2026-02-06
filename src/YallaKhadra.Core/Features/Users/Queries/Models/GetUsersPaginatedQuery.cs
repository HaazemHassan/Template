using MediatR;
using YallaKhadra.Core.Bases.Pagination;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Features.Users.Queries.Models {
    public class GetUsersPaginatedQuery : PaginatedQuery, IRequest<PaginatedResult<GetUsersPaginatedResponse>> {

    }
}
