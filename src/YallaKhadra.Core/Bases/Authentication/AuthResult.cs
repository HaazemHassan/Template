using System.Text.Json.Serialization;
using YallaKhadra.Core.Features.Users.Queries.Responses;

namespace YallaKhadra.Core.Bases.Authentication {
    public class AuthResult {

        public AuthResult(string accessToken, RefreshTokenDTO refreshToken, GetUserByIdResponse user) {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            User = user;
        }
        public string AccessToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; }

        public GetUserByIdResponse User { get; set; }
    }


    public class RefreshTokenDTO {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
