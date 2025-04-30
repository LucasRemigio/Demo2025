// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;

namespace engimatrix.Views;

public class HereGeocodedResponse : GenericResponse
{
    public HereGeocodeItemResponse? geocoded { get; set; }

    public HereGeocodedResponse(HereGeocodeItemResponse? geocoded, int result_code, string language) : base(result_code, language)
    {
        this.geocoded = geocoded;
    }

    public HereGeocodedResponse(int result_code, string language) : base(result_code, language)
    {
    }
}

public class HereRoutesResponse : GenericResponse
{
    public HereRoutesItemResponse? routes { get; set; }

    public HereRoutesResponse(HereRoutesItemResponse? routes, int result_code, string language) : base(result_code, language)
    {
        this.routes = routes;
    }

    public HereRoutesResponse(int result_code, string language) : base(result_code, language)
    {
    }
}