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
[Route("api/primavera-invoices")]
public class PrimaveraInvoiceController : ControllerBase
{
    [HttpGet]
    [Route("client/{client_code}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientPrimaveraInvoicesResponse>> GetClientPrimaveraInvoices(string client_code)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);
        if (string.IsNullOrEmpty(client_code) || string.IsNullOrEmpty(executer_user))
        {
            return new ClientPrimaveraInvoicesResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        try
        {
            List<MFPrimaveraInvoiceItem> invoices = await PrimaveraInvoiceModel.GetPrimaveraInvoicesByClientCodeLastYear(client_code);
            AveragePaymentTimeItem? averagePaymentTime = SyncPrimaveraRatingsModel.CalculateAveragePaymentTime(invoices);
            MFPrimaveraInvoiceTotalItem? invoiceTotal = PrimaveraInvoiceModel.CalculateInvoicesTotal(invoices);

            return new ClientPrimaveraInvoicesResponse(invoices, averagePaymentTime, invoiceTotal, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetClientPrimaveraOrders endpoint - Error - " + e);
            return new ClientPrimaveraInvoicesResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
