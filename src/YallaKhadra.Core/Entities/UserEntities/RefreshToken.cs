namespace YallaKhadra.Core.Entities {
    public class RefreshToken {
        public int Id { get; set; }
        public int UserId { get; set; }      //ApplicationUser not DomainUser
        public string? AccessTokenJTI { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? RevokationDate { get; set; }
        public bool IsActive => RevokationDate is null && !IsExpired;

    }
}
