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
using engimatrix.ModelObjs.Primavera;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/orders/documents")]
public class OrderPrimaveraDocumentController : ControllerBase
{
    [HttpGet]
    [Route("{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<OrderPrimaveraDocumentListResponse>> GetOrderPrimaveraDocuments(string orderToken)
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
            List<OrderPrimaveraDocumentItem> docs = OrderPrimaveraDocumentModel.GetOrderDocs(orderToken, executer_user);
            return new OrderPrimaveraDocumentListResponse(docs, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetOrderPrimaveraDocuments endpoint - Error - " + e);
            return new OrderPrimaveraDocumentListResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}