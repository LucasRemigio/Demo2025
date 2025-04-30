// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Net;
using System.Text.Json;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Connector;

public static class PrimaveraGenerics
{
    // this will make it possible to queue requests, as only one request can be made at a time
    private static readonly Dictionary<PrimaveraAccessType, SemaphoreSlim> _semaphores = new()
        {
            { PrimaveraAccessType.Read, new SemaphoreSlim(1, 1) },
            { PrimaveraAccessType.Write, new SemaphoreSlim(1, 1) }
        };


    /*  ===========================================================
    *               Authorization methods
    *  ===========================================================
    */

    public static async Task<string> LoginAsync(HttpMethod method)
    {
        // Define the endpoint and HttpClient
        string url = $"{ConfigManager.PrimaveraApiUrl}/token";

        using HttpClient client = new();

        string username = method == HttpMethod.Get ? ConfigManager.PrimaveraReadUsername : ConfigManager.PrimaveraWriteUsername;
        PrimaveraAccessType accessType = method == HttpMethod.Get ? PrimaveraAccessType.Read : PrimaveraAccessType.Write;
        Log.Debug(accessType.ToString());

        // Prepare the request data
        List<KeyValuePair<string, string>> requestData =
        [
            new("username", username),
            new("password", ConfigManager.PrimaveraPassword),
            new("company", ConfigManager.PrimaveraCompany),
            new("instance", ConfigManager.PrimaveraInstance),
            new("grant_type", ConfigManager.PrimaveraGrantType),
            new("line", ConfigManager.PrimaveraLine)
        ];

        // Create the request
        using HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(requestData)
        };

        try
        {
            // Send the request and ensure the response is successful
            HttpResponseMessage response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                string resContent = await response.Content.ReadAsStringAsync();
                throw new PrimaveraApiErrorException($"Error during login request: {resContent} - {response.ReasonPhrase} - {response.StatusCode}");
            }

            // Read and output the response content
            string responseContent = await response.Content.ReadAsStringAsync();

            // deserialize
            PrimaveraAccessItem? primaveraAccess = JsonSerializer.Deserialize<PrimaveraAccessItem>(responseContent);

            if (primaveraAccess == null)
            {
                Log.Debug("Error during the login request: Could not deserialize the response content");
                return string.Empty;
            }

            primaveraAccess.RetrievedAt = DateTime.UtcNow;

            bool isTokenValid = primaveraAccess.IsTokenValid();
            if (!isTokenValid)
            {
                Log.Debug("Error during the login request: The given token is invalid");
                return string.Empty;
            }

            PrimaveraAccessManager.AddOrUpdateAccess(accessType, primaveraAccess);

            return primaveraAccess.AccessToken;
        }
        catch (HttpRequestException ex)
        {
            Log.Debug($"Error during the login request: {ex.Message}");
            return string.Empty;
        }
    }

    public static async Task<bool> ValidateAccessAuthorization(HttpMethod method)
    {
        PrimaveraAccessType accessType = method == HttpMethod.Get ? PrimaveraAccessType.Read : PrimaveraAccessType.Write;
        PrimaveraAccessItem? authorization = PrimaveraAccessManager.GetAccess(accessType);
        if (authorization != null && authorization.IsTokenValid())
        {
            // Token is valid
            return true;
        }

        Log.Info($"Primavera ({accessType}) - Authorization is missing or invalid. Logging in the account...");

        string token = await LoginAsync(method);
        if (string.IsNullOrEmpty(token))
        {
            Log.Error($"Primavera ({accessType}) - Login failed. Exiting...");
            return false;
        }
        Log.Info($"Primavera ({accessType}) - Login successful. Updating context...");

        // update the context
        string url = "Plataforma/AtualizaContexto";
        PrimaveraUpdateContext updateContext = await PostResourceAsync(url, () => new PrimaveraUpdateContext());
        if (updateContext.StatusCode != 200)
        {
            Log.Error($"Primavera ({accessType}) - Error updating the context: {updateContext.ErrorMessage}");
            return false;
        }

        Log.Info($"Primavera ({accessType}) - Updated context successfully");
        return true;
    }

    /*  ===========================================================
    *               Generic request methods for list guids
    *  ===========================================================
    */

    public static async Task<T?> SendRequestAsync<T>(string endpoint, HttpMethod method, object? content)
    {
        if (!await ValidateAccessAuthorization(method))
        {
            Log.Debug("Error validating access token");
            return default;
        }

        string url = $"{ConfigManager.PrimaveraApiUrl}/{endpoint}";

        PrimaveraAccessType accessType = method == HttpMethod.Get ? PrimaveraAccessType.Read : PrimaveraAccessType.Write;
        PrimaveraAccessItem access = PrimaveraAccessManager.GetAccess(accessType)!;

        using HttpClient client = new();
        using HttpRequestMessage request = new(method, url)
        {
            Headers =
                {
                    { "Authorization", $"Bearer {access.AccessToken}" }
                },
            Content = content != null
                ? new StringContent(JsonSerializer.Serialize(content), System.Text.Encoding.UTF8, "application/json")
                : null
        };

        if (method != HttpMethod.Get)
        {
            string jsonPayload = JsonSerializer.Serialize(content);
            if (!string.IsNullOrEmpty(jsonPayload))
            {
                Log.Debug($"Request JSON: {jsonPayload}");
            }
        }

        try
        {
            await _semaphores[accessType].WaitAsync();

            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Log.Debug($"Error during the request to {url}: {responseContent} - {response.ReasonPhrase} - {response.StatusCode}");

                // check if status is unauthorized
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Login again
                    string token = await LoginAsync(method);
                    if (string.IsNullOrEmpty(token))
                    {
                        throw new PrimaveraApiErrorException($"Error during the request to {url}: {responseContent} - {response.ReasonPhrase} - {response.StatusCode}");
                    }

                    // Retry the request
                    response = await client.SendAsync(request);
                    responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        if (string.IsNullOrWhiteSpace(responseContent))
                        {
                            return default;
                        }
                        return JsonSerializer.Deserialize<T>(responseContent);
                    }
                }

                throw new PrimaveraApiErrorException($"Error during the request to {url}: {responseContent} - {response.ReasonPhrase} - {response.StatusCode}");

            }

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(responseContent);
        }
        catch (HttpRequestException ex)
        {
            Log.Debug($"Error during the request to {url}: {ex.Message}");
            throw new PrimaveraApiErrorException(ex.Message);
        }
        finally
        {
            _semaphores[accessType].Release();
        }
    }

    public static async Task<T?> SendRequestAsync<T>(string endpoint, HttpMethod method) => await SendRequestAsync<T>(endpoint, method, null);

    public static async Task<T?> GetListInfoAsync<T>(PrimaveraListParametersItem parameters)
    {
        string listParameters = string.Join(",", parameters.Offset, parameters.Limit, parameters.SqlWhere);
        string urlParameters = $"?listId={parameters.ListGuid}&listParameters={listParameters}";

        T? response = await SendRequestAsync<T>(
            $"Plataforma/Listas/CarregaLista/adhoc/{urlParameters}",
            HttpMethod.Get
        );

        return response;
    }

    public static async Task<PrimaveraListResponseItem<T>> GetListResourceAsync<T>(PrimaveraListParametersItem parameters, Func<T> defaultValueProvider)
    {
        PrimaveraListResponseItem<T>? result = await GetListInfoAsync<PrimaveraListResponseItem<T>>(parameters);

        if (result == null || result.IsError)
        {
            PrimaveraHelpers.PrimaveraListNames.TryGetValue(parameters.ListGuid, out string? listName);
            listName ??= "Unknown List";

            Log.Debug($"Failed to retrieve resource from the list {listName} with GUID {parameters.ListGuid}. Message: {result?.Message ?? "Unknown error"}");

            return new PrimaveraListResponseItem<T>
            {
                Fields = [],
                Data = [defaultValueProvider()],
                Message = result?.Message
            };
        }

        return result;
    }

    /*  ===========================================================
    *           GENERICS for predefined endpoints
    *   ===========================================================
    */

    public static async Task<T> GetResourceAsync<T>(string url, Func<T> defaultValueProvider)
    {
        T? result = await SendRequestAsync<T>(url, HttpMethod.Get);

        // check for nullity
        if (EqualityComparer<T>.Default.Equals(result, default))
        {
            Log.Debug($"Failed to retrieve resource from {url}. Returning a default value.");
            return defaultValueProvider();
        }

        return result!;
    }

    public static async Task<T> PostResourceAsync<T>(string url, Func<T> defaultValueProvider, object? content)
    {
        T? result = await SendRequestAsync<T>(url, HttpMethod.Post, content);

        // check for nullity
        if (EqualityComparer<T>.Default.Equals(result, default))
        {
            Log.Debug($"Failed to post resource to {url}. Returning a default value.");
            return defaultValueProvider();
        }

        return result!;
    }

    public static async Task<T> PostResourceAsync<T>(string url, Func<T> defaultValueProvider) => await PostResourceAsync(url, defaultValueProvider, null);

    public static async Task<bool> CheckExistenceAsync(string url)
    {
        bool? exists = await SendRequestAsync<bool>(url, HttpMethod.Get);

        if (exists == null)
        {
            Log.Debug($"Failed to check existence at {url}. Defaulting to 'false'.");
            return false;
        }

        return exists.Value;
    }
}
