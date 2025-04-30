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
[Route("api/segments")]
public class SegmentsController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<SegmentListResponse> GetSegments()
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
            return new SegmentListResponse(SegmentModel.GetSegments(executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSegments endpoint - Error - " + e);
            return new SegmentListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{segmentId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<SegmentItemResponse> GetSegmentById(int segmentId)
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
            return new SegmentItemResponse(SegmentModel.GetSegmentById(segmentId, executer_user), ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("GetSegmentById endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("GetSegmentById endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<SegmentItemResponse> CreateSegment(SegmentCreateRequest segmentReq)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!segmentReq.IsValid())
        {
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            SegmentItem segmentItem = new SegmentItem(segmentReq.name);
            SegmentModel.CreateSegment(segmentItem, executer_user);

            return new SegmentItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("CreateSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (ItemAlreadyExistsException e)
        {
            Log.Error("CreateSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("CreateSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{segmentId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<SegmentItemResponse> UpdateSegment(int segmentId, SegmentUpdateRequest segmentReq)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!segmentReq.IsValid())
        {
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            SegmentItem segmentItem = new SegmentItem(segmentId, segmentReq.name);
            SegmentModel.UpdateSegment(segmentItem, executer_user);

            return new SegmentItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("UpdateSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (ItemAlreadyExistsException e)
        {
            Log.Error("UpdateSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpDelete]
    [Route("{segmentId}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<SegmentItemResponse> DeleteSegment(int segmentId)
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
            SegmentModel.DeleteSegment(segmentId, executer_user);

            return new SegmentItemResponse(ResponseSuccessMessage.Success, language);
        }
        catch (InputNotValidException e)
        {
            Log.Error("DeleteSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InvalidArgs, language);
        }
        catch (Exception e)
        {
            Log.Error("DeleteSegment endpoint - Error - " + e);
            return new SegmentItemResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
