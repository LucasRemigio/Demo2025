
namespace engimatrix.ModelObjs;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class HereRoutesItemResponse
{
    [JsonPropertyName("routes")]
    public List<Route> Routes { get; set; }

    [JsonPropertyName("notice")]
    public List<Notice> Notice { get; set; }
}

public class Route
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("sections")]
    public List<Section> Sections { get; set; }
}

public class Section
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("departure")]
    public Departure Departure { get; set; }

    [JsonPropertyName("arrival")]
    public Arrival Arrival { get; set; }

    [JsonPropertyName("summary")]
    public Summary Summary { get; set; }

    [JsonPropertyName("transport")]
    public Transport Transport { get; set; }
}

public class Departure
{
    [JsonPropertyName("place")]
    public Place Place { get; set; }
}

public class Arrival
{
    [JsonPropertyName("place")]
    public Place Place { get; set; }
}

public class Place
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("location")]
    public Coordinate Location { get; set; }

    [JsonPropertyName("originalLocation")]
    public Coordinate OriginalLocation { get; set; }
}

public class Coordinate
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
}

public class Summary
{
    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("baseDuration")]
    public int BaseDuration { get; set; }
}

public class Transport
{
    [JsonPropertyName("mode")]
    public string Mode { get; set; }
}

public class Notice
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}
