using Microsoft.EntityFrameworkCore;
using YallaKhadra.Core.Entities;
using YallaKhadra.Infrastructure.Abstracts;
using YallaKhadra.Infrastructure.Data;

namespace YallaKhadra.Infrastructure.Repositories {
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository {

        private readonly DbSet<RefreshToken> _refreshTokens;


        public RefreshTokenRepository(AppDbContext context) : base(context) {
            _refreshTokens = context.Set<RefreshToken>();
        }

    }
}
