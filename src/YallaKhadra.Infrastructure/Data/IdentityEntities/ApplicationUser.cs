using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using YallaKhadra.Core.Entities.UserEntities;

namespace YallaKhadra.Core.Entities.IdentityEntities {
    public class ApplicationUser : IdentityUser<int> {

        public int? DomainUserId { get; set; }

        [ForeignKey(nameof(DomainUserId))]
        public virtual DomainUser? DomainUser { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
