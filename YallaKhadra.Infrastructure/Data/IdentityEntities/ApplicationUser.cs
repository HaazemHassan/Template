using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace YallaKhadra.Core.Entities.IdentityEntities {
    public class ApplicationUser : IdentityUser<int> {

        public int? DomainUserId { get; set; }

        [ForeignKey(nameof(DomainUserId))]
        public virtual User? DomainUser { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
