using System.Text.Json.Serialization;

namespace CMS.Dtos;

public class FacebookAppAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
}
