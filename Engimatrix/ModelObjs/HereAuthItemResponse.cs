
namespace engimatrix.ModelObjs;

using System.Text.Json.Serialization;

public class HereAuthItemResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; }

    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }
}
