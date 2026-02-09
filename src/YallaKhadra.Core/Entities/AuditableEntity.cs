using YallaKhadra.Core.Entities.Abstracts;

namespace YallaKhadra.Core.Entities {
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity {
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
