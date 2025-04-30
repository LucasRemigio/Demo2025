// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;
using System.Text.Json;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Connector;

public static class HereApiUrlBuilder
{

    /// <summary>
    /// Builds the URL for the HERE routing API with the given parameters.
    /// </summary>
    /// <param name="origin">Origin coordinates in "latitude,longitude" format.</param>
    /// <param name="destination">Destination coordinates in "latitude,longitude" format.</param>
    /// <param name="transportMode">Mode of transportation (e.g., "car").</param>
    /// <returns>The fully constructed URL for the GET request.</returns>
    public static string BuildRoutesUrl(string origin, string destination, HereRouteTransportMode transportMode)
    {
        if (!HereApiHelper.IsCoordinateValid(origin))
        {
            throw new ArgumentException("Invalid origin coordinates", nameof(origin));
        }
        if (!HereApiHelper.IsCoordinateValid(destination))
        {
            throw new ArgumentException("Invalid destination coordinates", nameof(destination));
        }

        string baseUrl = "https://router.hereapi.com/v8/routes?";

        // Construct the query string.
        // Note: Ensure that the origin and destination values are properly URL encoded if needed.
        StringBuilder url = new(baseUrl);
        url.Append($"transportMode={transportMode.ToString().ToLower()}");
        url.Append($"&origin={origin}");
        url.Append($"&destination={destination}");
        url.Append("&return=summary");

        return url.ToString();
    }

    /// <summary>
    /// Builds the URL for the HERE Geocoding API using the given free-form query.
    /// </summary>
    /// <param name="query">The free-form address string to geocode.</param>
    /// <returns>The fully constructed URL for the GET request.</returns>
    public static string BuildGeocodeUrl(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentNullException(nameof(query));
        }

        string baseUrl = "https://geocode.search.hereapi.com/v1/geocode?";

        // URL encode the query string.
        string encodedQuery = Uri.EscapeDataString(query);

        StringBuilder url = new(baseUrl);
        url.Append($"q={encodedQuery}");

        return url.ToString();
    }
}