// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs.Primavera;

public class PrimaveraAccessItem
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    public DateTime RetrievedAt { get; set; }

    public PrimaveraAccessItem(string accessToken, string tokenType, int expiresIn)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
    }

    // Check if the token is valid
    public bool IsTokenValid()
    {
        double marginPercentage = 0.1 * ExpiresIn;
        DateTime expiryTime = RetrievedAt.AddSeconds(ExpiresIn - marginPercentage);
        return DateTime.UtcNow < expiryTime;
    }
}
