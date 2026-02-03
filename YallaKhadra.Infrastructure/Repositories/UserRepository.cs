using Microsoft.EntityFrameworkCore;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Entities;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Infrastructure.Repositories {
    public class UserRepository : GenericRepository<User>, IUserRepository {

        private readonly DbSet<User> _users;


        public UserRepository(AppDbContext context) : base(context) {
            _users = context.Set<User>();
        }

    }
}
