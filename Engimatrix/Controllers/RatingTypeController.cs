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

namespace Engimatrix.Controllers
{
    [ApiController]
    [Route("api/ratings/types")]
    public class RatingTypeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingTypeListResponse> GetRatingTypes()
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
                return new RatingTypeListResponse(RatingTypeModel.GetRatingTypes(executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetRatingTypes endpoint - Error - " + e);
                return new RatingTypeListResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("{ratingType:regex(^client$|^order$)}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingTypeListResponse> GetClientRatingTypes(string ratingType)
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
                IRatingTypeStrategy strategy = ratingType switch
                {
                    "client" => new ClientRatingTypes(),
                    "order" => new OrderRatingTypes(),
                    _ => throw new ArgumentException($"Given rating type {ratingType} is invalid")
                };

                List<RatingTypeItem> ratingTypes = strategy.Execute(executer_user);

                return new RatingTypeListResponse(ratingTypes, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetClientRatingTypes endpoint - Error - " + e);
                return new RatingTypeListResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingTypeItemResponse> GetRatingTypeById(int id)
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
                return new RatingTypeItemResponse(RatingTypeModel.GetRatingType(id, executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetRatingTypeById endpoint - Error - " + e);
                return new RatingTypeItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingTypeItemResponse> CreateRatingType([FromBody] RatingTypeCreateRequest ratingType)
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
                RatingTypeItem rating = new(ratingType.description, ratingType.slug, ratingType.weight);
                RatingTypeModel.CreateRatingType(rating, executer_user);

                return new RatingTypeItemResponse(ResponseSuccessMessage.Success, language);
            }
            catch (ItemAlreadyExistsException e)
            {
                Log.Error("CreateRatingType - RatingType - Error - " + e);
                return BadRequest(new RatingTypeItemResponse(ResponseErrorMessage.InvalidArgs, language));
            }
            catch (Exception e)
            {
                Log.Error("CreateRatingType endpoint - Error - " + e);
                return new RatingTypeItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingTypeItemResponse> UpdateRatingType(int id, [FromBody] RatingTypeCreateRequest ratingType)
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
                RatingTypeItem rating = new(id, ratingType.description, ratingType.slug, ratingType.weight);

                RatingTypeModel.UpdateRatingType(rating, executer_user);

                return new RatingTypeItemResponse(ResponseSuccessMessage.Success, language);
            }
            catch (InputNotValidException e)
            {
                Log.Error("UpdateRatingType - RatingType - Error - " + e);
                return BadRequest(new RatingTypeItemResponse(ResponseErrorMessage.InvalidArgs, language));
            }
            catch (Exception e)
            {
                Log.Error("UpdateRatingType endpoint - Error - " + e);
                return new RatingTypeItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingTypeItemResponse> DeleteRatingType(int id)
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
                RatingTypeModel.DeleteRatingType(id, executer_user);

                return new RatingTypeItemResponse(ResponseSuccessMessage.Success, language);
            }
            catch (InputNotValidException e)
            {
                Log.Error("DeleteRatingType - RatingType - Error - " + e);
                return BadRequest(new RatingTypeItemResponse(ResponseErrorMessage.InvalidArgs, language));
            }
            catch (ItemAlreadyExistsException e)
            {
                Log.Error("DeleteRatingType - ItemAlreadyExists - Error - " + e);
                return BadRequest(new RatingTypeItemResponse(ResponseErrorMessage.InvalidArgs, language));
            }
            catch (Exception e)
            {
                Log.Error("DeleteRatingType endpoint - Error - " + e);
                return new RatingTypeItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
