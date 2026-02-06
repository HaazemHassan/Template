using Microsoft.EntityFrameworkCore;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts.Repositories;
using YallaKhadra.Core.Entities.UserEntities;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Infrastructure.Repositories {
    public class UserRepository : GenericRepository<DomainUser>, IUserRepository {

        private readonly DbSet<DomainUser> _users;


        public UserRepository(AppDbContext context) : base(context) {
            _users = context.Set<DomainUser>();
        }

    }
}
