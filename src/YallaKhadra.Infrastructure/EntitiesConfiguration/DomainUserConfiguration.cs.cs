using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YallaKhadra.Core.Entities.UserEntities;

namespace YallaKhadra.Infrastructure.EntitiesConfiguration {
    public class DomainUserConfiguration : IEntityTypeConfiguration<DomainUser> {
        public void Configure(EntityTypeBuilder<DomainUser> builder) {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}
