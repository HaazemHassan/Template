using MediatR;
using YallaKhadra.Core.Extensions;

namespace YallaKhadra.Core.Behaviors {
    public class TrimmingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : notnull, IRequest<TResponse> {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
            request.ApplyTrim();
            return await next(cancellationToken);
        }

    }
}


