// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Filters;
using engimatrix.ResponseMessages;
using engimatrix.Views;
using Microsoft.AspNetCore.Mvc;
using engimatrix.Models;
using engimatrix.Utils;
using static engimatrix.Config.RatingConstants;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/rating/criterias")]
public class RatingCriteriaController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<RatingCriteriaDTOListResponse> GetRatingCriterias()
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
            return new RatingCriteriaDTOListResponse(RatingCriteriaModel.GetRatingsDTO(executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetRatingCriterias endpoint - Error - " + e);
            return new RatingCriteriaDTOListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<RatingCriteriaDTOResponse> GetRatingCriteriaById(int id)
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
            return new RatingCriteriaDTOResponse(RatingCriteriaModel.GetRatingCriteriaByIdDTO(id, executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetRatingCriteriaById endpoint - Error - " + e);
            return new RatingCriteriaDTOResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<RatingCriteriaDTOResponse> CreateRatingCriteria([FromBody] RatingCriteriaCreateRequest ratingCriteriaReq)
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
            RatingCriteriaItem rating = new(ratingCriteriaReq.id, ratingCriteriaReq.rating_type_id, ratingCriteriaReq.rating, ratingCriteriaReq.criteria);

            RatingCriteriaModel.CreateRatingCriteria(rating, executer_user);

            return new RatingCriteriaDTOResponse(ResponseSuccessMessage.Success, language);
        }
        catch (RatingNotValidException e)
        {
            Log.Error("CreateRatingCriteria - Rating - Error - " + e);
            return BadRequest(new RatingCriteriaDTOResponse(ResponseErrorMessage.InvalidRating, language));
        }
        catch (Exception e)
        {
            Log.Error("CreateRatingCriteria endpoint - Error - " + e);
            return new RatingCriteriaDTOResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<RatingCriteriaDTOResponse> UpdateRatingCriteria(int id, [FromBody] RatingCriteriaCreateRequest ratingCriteriaReq)
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
            RatingCriteriaItem rating = new(id, ratingCriteriaReq.rating_type_id, ratingCriteriaReq.rating, ratingCriteriaReq.criteria);

            RatingCriteriaModel.UpdateRatingCriteria(rating, executer_user);

            return new RatingCriteriaDTOResponse(ResponseSuccessMessage.Success, language);
        }
        catch (RatingNotValidException e)
        {
            Log.Error("UpdateRatingCriteria - Rating - Error - " + e);
            return BadRequest(new RatingCriteriaDTOResponse(ResponseErrorMessage.InvalidRating, language));
        }
        catch (Exception e)
        {
            Log.Error("UpdateRatingCriteria endpoint - Error - " + e);
            return new RatingCriteriaDTOResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<RatingCriteriaDTOResponse> DeleteRatingCriteria(int id)
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
            RatingCriteriaModel.DeleteRatingCriteria(id, executer_user);

            return new RatingCriteriaDTOResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("DeleteRatingCriteria endpoint - Error - " + e);
            return new RatingCriteriaDTOResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
