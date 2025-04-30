// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Connector;

public static class HereApiAuth
{
    /// <summary>
    /// Retrieves an OAuth access token from the HERE authentication endpoint using OAuth 1.0 authentication.
    /// </summary>
    /// <returns>An OAuthTokenResponse containing the access token details.</returns>
    public static async Task<HereAuthItem?> LoginAsync()
    {
        string url = "https://account.api.here.com/oauth2/token";
        string consumerKey = ConfigManager.HereAppKeyId;
        string consumerSecret = ConfigManager.HereAppKeySecret;
        string grantType = "client_credentials";

        string oauthTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string oauthNonce = Guid.NewGuid().ToString("N").Substring(0, 10);
        string oauthSignatureMethod = "HMAC-SHA1";
        string oauthVersion = "1.0";

        SortedDictionary<string, string> parameters = new()
        {
            { "oauth_consumer_key", consumerKey },
            { "oauth_nonce", oauthNonce },
            { "oauth_signature_method", oauthSignatureMethod },
            { "oauth_timestamp", oauthTimestamp },
            { "oauth_version", oauthVersion }
        };

        string normalizedParameters = string.Join("&", parameters.Select(kvp =>
            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
        string signatureBaseString = $"POST&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(normalizedParameters)}";
        string signingKey = $"{Uri.EscapeDataString(consumerSecret)}&";

        string oauthSignature;
        using (HMACSHA1 hmac = new(Encoding.ASCII.GetBytes(signingKey)))
        {
            oauthSignature = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString)));
        }

        string authHeader = $"OAuth oauth_consumer_key=\"{Uri.EscapeDataString(consumerKey)}\", " +
                            $"oauth_signature_method=\"{oauthSignatureMethod}\", " +
                            $"oauth_timestamp=\"{oauthTimestamp}\", " +
                            $"oauth_nonce=\"{oauthNonce}\", " +
                            $"oauth_version=\"{oauthVersion}\", " +
                            $"oauth_signature=\"{Uri.EscapeDataString(oauthSignature)}\"";

        using HttpClient client = new();
        using HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = new StringContent(
                $"{{\"grantType\":\"{grantType}\",\"clientId\":\"{consumerKey}\",\"clientSecret\":\"{consumerSecret}\"}}",
                Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Authorization", authHeader);

        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        HereAuthItem? tokenResponse = JsonSerializer.Deserialize<HereAuthItem>(responseContent);
        if (tokenResponse == null)
        {
            Log.Error("Failed to deserialize token response");
            return null;
        }

        ConfigManager.HereAuth = new(tokenResponse.AccessToken, tokenResponse.TokenType, tokenResponse.ExpiresIn);
        return tokenResponse;
    }

    public static bool IsAuthenticationValid()
    {
        HereAuthItem? auth = ConfigManager.HereAuth;
        if (auth == null)
        {
            return false;
        }

        return auth.IsAuthenticationValid();
    }

    public async static Task<bool> Authenticate()
    {
        if (!IsAuthenticationValid())
        {
            return await LoginAsync() != null;
        }

        return true;
    }
}