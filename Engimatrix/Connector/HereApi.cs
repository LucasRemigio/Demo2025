// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Connector;

public static class HereApi
{
    public static async Task<T?> SendAuthenticatedGetRequest<T>(string url)
    {
        // Ensure we're authenticated before sending the request.
        if (!await HereApiAuth.Authenticate())
        {
            throw new Exception("Invalid HERE API credentials");
        }

        using HttpClient client = new();
        HttpRequestMessage request = new(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue(ConfigManager.HereAuth!.TokenType, ConfigManager.HereAuth!.AccessToken);

        try
        {
            HttpResponseMessage response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error during the request to {url}: {responseContent} - {response.ReasonPhrase} - {response.StatusCode}";
                Log.Error(errorMessage);
                throw new Exception(errorMessage);
            }

            return JsonSerializer.Deserialize<T>(responseContent);
        }
        catch (HttpRequestException ex)
        {
            Log.Error($"Error during the route calculation request: {ex.Message}");
            throw new Exception("Error during the route calculation request", ex);
        }
    }

    /// <summary>
    /// Calculates a route using the HERE Routing API.
    /// </summary>
    /// <param name="origin">
    /// The starting point coordinates in "latitude,longitude" format.
    /// Example: "52.5308,13.3847"
    /// </param>
    /// <param name="destination">
    /// The destination coordinates in "latitude,longitude" format.
    /// Example: "52.5264,13.3686"
    /// </param>
    /// <param name="transportMode">
    /// The mode of transport (e.g., "car", "pedestrian", "truck", etc.). Defaults to "car".
    /// </param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the route response.</returns>
    public static async Task<HereRoutesItemResponse?> Route(string origin, string destination, HereRouteTransportMode transportMode)
    {
        string url = HereApiUrlBuilder.BuildRoutesUrl(origin, destination, transportMode);
        return await SendAuthenticatedGetRequest<HereRoutesItemResponse>(url);
    }

    /// <summary>
    /// Calculates a route using the HERE Routing API.
    /// </summary>
    /// <param name="destination">
    /// The destination coordinates in "latitude,longitude" format.
    /// Example: "52.5264,13.3686"
    /// </param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the route response.</returns>
    public static async Task<HereRoutesItemResponse?> RouteFromMasterFerro(string destination)
    {
        string origin = ConfigManager.MFLat + "," + ConfigManager.MFLng;
        HereRouteTransportMode transportMode = HereRouteTransportMode.Car;

        return await Route(origin, destination, transportMode);
    }

    /// <summary>
    /// Geocodes a free-form address string using the HERE Geocoding API.
    /// </summary>
    /// <param name="query">
    /// The free-form address to geocode.
    /// Example: "Invalidenstr 117, Berlin"
    /// </param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the geocode response.</returns>
    public static async Task<HereGeocodeItemResponse?> Geocode(string query)
    {
        string url = HereApiUrlBuilder.BuildGeocodeUrl(query);
        return await SendAuthenticatedGetRequest<HereGeocodeItemResponse>(url);
    }
}