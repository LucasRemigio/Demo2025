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
    [Route("api/ratings/discounts")]
    public class RatingDiscountsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingDiscountListResponse> GetRatingDiscounts()
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
                List<RatingDiscountItem> ratingDiscounts = RatingDiscountModel.GetRatingDiscounts(executer_user);
                return new RatingDiscountListResponse(ratingDiscounts, ResponseSuccessMessage.Success, language);
            }
            catch (Exception e)
            {
                Log.Error("GetRatingDiscount endpoint - Error - " + e);
                return new RatingDiscountListResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpGet]
        [Route("{rating}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingDiscountItemResponse> GetRatingDiscountByRating(char rating)
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
                return new RatingDiscountItemResponse(RatingDiscountModel.GetRatingDiscountByRating(rating, executer_user), ResponseSuccessMessage.Success, language);
            }
            catch (RatingNotValidException e)
            {
                Log.Error("GetRatingDiscountsByRating - Rating - Error - " + e);
                return BadRequest(new RatingDiscountItemResponse(ResponseErrorMessage.InvalidRating, language));
            }
            catch (Exception e)
            {
                Log.Error("GetRatingDiscountsByRating endpoint - Error - " + e);
                return new RatingDiscountItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPost]
        [Route("")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingDiscountItemResponse> CreateRating(RatingDiscountCreateRequest ratingReq)
        {
            string language = this.Request.Headers["client-lang"];
            if (string.IsNullOrEmpty(language))
            {
                language = ConfigManager.defaultLanguage;
            }

            if (!ratingReq.IsValid())
            {
                return BadRequest(new RatingDiscountListResponse(ResponseErrorMessage.InvalidRating, language));
            }

            string token = this.Request.Headers["Authorization"];
            string executer_user = UserModel.GetUserByToken(token);

            try
            {
                RatingDiscountItem rating = new(ratingReq.rating, ratingReq.percentage);
                RatingDiscountModel.CreateRatingDiscount(rating, executer_user);

                return new RatingDiscountItemResponse(ResponseSuccessMessage.Success, language);
            }
            catch (RatingNotValidException e)
            {
                Log.Error("GetRatingDiscountsByRating - Rating - Error - " + e);
                return BadRequest(new RatingDiscountItemResponse(ResponseErrorMessage.InvalidRating, language));
            }
            catch (ItemAlreadyExistsException e)
            {
                Log.Error("UpdateRating - ItemAlreadyExists - Error - " + e);
                return BadRequest(new RatingDiscountItemResponse(ResponseErrorMessage.InvalidArgs, language));
            }
            catch (Exception e)
            {
                Log.Error("GetRatingDiscountsByRating endpoint - Error - " + e);
                return new RatingDiscountItemResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpPut]
        [Route("{rating}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingDiscountListResponse> UpdateRatingDiscount(char rating, [FromBody] RatingDiscountUpdateRequest ratingReq)
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
                RatingDiscountItem ratingItem = new(rating, ratingReq.percentage);

                RatingDiscountModel.UpdateRatingDiscount(ratingItem, executer_user);

                return new RatingDiscountListResponse(ResponseSuccessMessage.Success, language);
            }
            catch (ItemAlreadyExistsException e)
            {
                Log.Error("UpdateRatingDiscount - ItemAlreadyExists - Error - " + e);
                return BadRequest(new RatingDiscountListResponse(ResponseErrorMessage.InvalidArgs, language));
            }
            catch (Exception e)
            {
                Log.Error("UpdateRatingDiscount endpoint - Error - " + e);
                return new RatingDiscountListResponse(ResponseErrorMessage.InternalError, language);
            }
        }

        [HttpDelete]
        [Route("{rating}")]
        [RequestLimit]
        [ValidateReferrer]
        [Authorize]
        public ActionResult<RatingDiscountListResponse> DeleteRatingDiscount(char rating)
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
                RatingDiscountModel.DeleteRatingDiscount(rating, executer_user);

                return new RatingDiscountListResponse(ResponseSuccessMessage.Success, language);
            }
            catch (RatingNotValidException e)
            {
                Log.Error("DeleteRatingDiscount - Rating - Error - " + e);
                return BadRequest(new RatingDiscountListResponse(ResponseErrorMessage.InvalidRating, language));
            }
            catch (Exception e)
            {
                Log.Error("DeleteRatingDiscount endpoint - Error - " + e);
                return new RatingDiscountListResponse(ResponseErrorMessage.InternalError, language);
            }
        }
    }
}
