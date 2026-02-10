using YallaKhadra.Core.Abstracts.CoreAbstracts.Services;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;

namespace YallaKhadra.Core.Services {
    public class DomainUserService : IDomainUserService {
        private readonly IUnitOfWork _unitOfWork;

        public DomainUserService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }


    }
}
