// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using Engimatrix.Models;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Connector;
using Engimatrix.ModelObjs;
using Engimatrix.Views;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using System.Threading.Tasks;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/here")]
public class LocationController : ControllerBase
{
    [HttpGet]
    [Route("geocode")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<HereGeocodedResponse>> GeocodeAddress(AddressRequest address)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            HereGeocodeItemResponse? geocode = await HereApi.Geocode(address.address);

            return new HereGeocodedResponse(geocode, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new HereGeocodedResponse(ResponseErrorMessage.InternalError, language);
        }
    }


    [HttpGet]
    [Route("route")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<HereRoutesResponse>> CalculateRoute(AddressRequest address)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            HereRoutesItemResponse? routes = await HereApi.RouteFromMasterFerro(address.address);

            return new HereRoutesResponse(routes, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new HereRoutesResponse(ResponseErrorMessage.InternalError, language);
        }
    }


    [HttpGet]
    [Route("login")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<GenericResponse>> Login(AddressRequest address)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            await HereApiAuth.LoginAsync();

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

}
