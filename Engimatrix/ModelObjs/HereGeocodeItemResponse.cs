
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace engimatrix.ModelObjs;

public class HereGeocodeItemResponse
{
    [JsonPropertyName("items")]
    public List<GeocodeItem> Items { get; set; }
}

public class GeocodeItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("resultType")]
    public string ResultType { get; set; }

    // This property is optional and may not be present in all responses.
    [JsonPropertyName("houseNumberType")]
    public string HouseNumberType { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; }

    [JsonPropertyName("access")]
    public List<Position> Access { get; set; }

    [JsonPropertyName("mapView")]
    public MapView MapView { get; set; }

    [JsonPropertyName("scoring")]
    public Scoring Scoring { get; set; }
}

public class Address
{
    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }

    [JsonPropertyName("countryName")]
    public string CountryName { get; set; }

    [JsonPropertyName("stateCode")]
    public string StateCode { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("countyCode")]
    public string CountyCode { get; set; }

    [JsonPropertyName("county")]
    public string County { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("district")]
    public string District { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("houseNumber")]
    public string HouseNumber { get; set; }
}

public class Position
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
}

public class MapView
{
    [JsonPropertyName("west")]
    public double West { get; set; }

    [JsonPropertyName("south")]
    public double South { get; set; }

    [JsonPropertyName("east")]
    public double East { get; set; }

    [JsonPropertyName("north")]
    public double North { get; set; }
}

public class Scoring
{
    [JsonPropertyName("queryScore")]
    public double QueryScore { get; set; }

    [JsonPropertyName("fieldScore")]
    public FieldScore FieldScore { get; set; }
}

public class FieldScore
{
    [JsonPropertyName("city")]
    public double City { get; set; }

    [JsonPropertyName("streets")]
    public List<double> Streets { get; set; }

    [JsonPropertyName("houseNumber")]
    public double HouseNumber { get; set; }
}