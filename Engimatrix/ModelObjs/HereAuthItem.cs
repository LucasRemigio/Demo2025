
namespace engimatrix.ModelObjs;

using System.Text.Json.Serialization;
public class HereAuthItem
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; }
    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }
    public DateTime RetrievedAt { get; set; }

    public HereAuthItem(string accessToken, string tokenType, int expiresIn)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
        RetrievedAt = DateTime.Now;
    }

    public HereAuthItem()
    {
    }

    public bool IsTokenValid()
    {
        if (string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(TokenType) || ExpiresIn <= 0)
        {
            return false;
        }

        return true;
    }

    public bool IsAuthenticationValid()
    {
        if (!IsTokenValid())
        {
            return false;
        }

        DateTime expirationTime = RetrievedAt.AddSeconds(ExpiresIn);

        if (expirationTime < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        return $"AccessToken: {AccessToken}, TokenType: {TokenType}, ExpiresIn: {ExpiresIn}";
    }


}
