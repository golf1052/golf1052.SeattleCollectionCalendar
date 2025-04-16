using System.Text.Json.Serialization;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class GuestAuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        public string Scope { get; set; } = string.Empty;
        public long Created { get; set; }
        public UserInfo User { get; set; } = null!;
        public string Jti { get; set; } = string.Empty;
    }

    public class UserInfo
    {
        public string CustomerId { get; set; } = string.Empty;
        public bool SocialInd { get; set; }
        public int InvalidLoginCount { get; set; }
        public bool EscrowUserStatus { get; set; }
    }
}
