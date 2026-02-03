using System.Text.Json.Serialization;

namespace YallaKhadra.Core.Features.Users {
    public class UserResponse {

        [JsonPropertyOrder(-1)]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int PointsBalance { get; set; }



    }
}
