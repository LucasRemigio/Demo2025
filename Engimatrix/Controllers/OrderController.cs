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
using engimatrix.ModelObjs.Primavera;
using static engimatrix.Views.Filtering;
using System.Runtime.CompilerServices;

namespace Engimatrix.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    [HttpPut]
    [Route("{orderToken}/address")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<OrderUpdateAddressResponse>> UpdateOrderAddress(OrderAddressRequest orderAddressReq, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!orderAddressReq.IsValid())
        {
            return new OrderUpdateAddressResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];

        string executer_user = "Anonymous Client";
        if (!String.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            OrderModel.UpdateOrderAddress(orderToken, orderAddressReq, executer_user);

            (DestinationDetailsItem details, OrderRatingItem logisticRating) = await OrderRatingModel.PatchLogisticRating(orderToken, executer_user);

            return new OrderUpdateAddressResponse(details, logisticRating, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new OrderUpdateAddressResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{orderToken}/big-transport")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<GenericResponse> PatchBigTransportToAddress(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        string token = this.Request.Headers["Authorization"];

        string executer_user = "Anonymous Client";
        if (!String.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            OrderModel.PatchBigTransport(orderToken, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("cancel/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> CancelOrder(CancelOrderRequest cancelReq, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!cancelReq.IsValid())
        {
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            OrderModel.CancelOrder(orderToken, cancelReq.cancel_reason_id, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("confirm/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<OrderPrimaveraDocumentResponse>> ConfirmOrder(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        string token = this.Request.Headers["Authorization"];

        string execute_user = string.Empty;
        if (!String.IsNullOrEmpty(token))
        {
            execute_user = UserModel.GetUserByToken(token);
        }

        try
        {
            await OrderModel.ConfirmOrder(orderToken, execute_user);

            return new OrderPrimaveraDocumentResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new OrderPrimaveraDocumentResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("{orderToken}/pending-client-confirmation")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<OrderPrimaveraDocumentResponse> SetQuotationAsClientPending(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        string token = this.Request.Headers["Authorization"];

        string execute_user = string.Empty;
        if (!String.IsNullOrEmpty(token))
        {
            execute_user = UserModel.GetUserByToken(token);
        }

        try
        {
            int statusId = StatusConstants.StatusCode.PENDENTE_CONFIRMACAO_CLIENTE;
            OrderModel.PatchStatus(orderToken, statusId, execute_user);

            return new OrderPrimaveraDocumentResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new OrderPrimaveraDocumentResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPut]
    [Route("no-auth/adjudicate/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public async Task<ActionResult<OrderPrimaveraDocumentResponse>> AdjudicateOrderNoAuth(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        string token = this.Request.Headers["Authorization"];

        string execute_user = string.Empty;
        if (!String.IsNullOrEmpty(token))
        {
            execute_user = UserModel.GetUserByToken(token);
        }

        try
        {
            await OrderModel.AdjudicateOrder(orderToken, execute_user);

            return new OrderPrimaveraDocumentResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new OrderPrimaveraDocumentResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{orderToken}/update-credit")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> UpdateOrderCredit(AcceptCreditRequest req, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        string token = this.Request.Headers["Authorization"];

        string execute_user = UserModel.GetUserByToken(token);

        if (!UserModel.IsUserAdminOrSupervisor(execute_user))
        {
            return new GenericResponse(ResponseErrorMessage.Unauthorized, language);
        }

        try
        {
            // Set the appropriate status based on the accept parameter
            int status = req.accepted
                ? StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR
                : StatusConstants.StatusCode.CREDITO_REJEITADO;

            OrderModel.PatchStatus(orderToken, status, execute_user);

            Log.Debug($"Credit for order {orderToken} was {(req.accepted ? "accepted" : "rejected")} by {execute_user}");
            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error($"Update credit endpoint - Error - {e}");
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{orderToken}/client")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<ClientPatchResponse>> PatchOrderClient(EditOrderClientRequest req, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!req.IsValid())
        {
            return new ClientPatchResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            (AddressFillingDetails? address, List<OrderProductItem> updatedProducts) = await OrderModel.PatchClient(orderToken, req.client_code, req.client_nif, executer_user);

            DestinationDetailsItem? details = null;
            OrderRatingItem? logisticRating = null;
            if (address != null)
            {
                (details, logisticRating) = await OrderRatingModel.PatchLogisticRating(orderToken, executer_user);
            }

            return new ClientPatchResponse(updatedProducts, address, details, logisticRating, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new ClientPatchResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{orderToken}/observations")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> PatchObservationsAndContact(OrderObservationsRequest obs, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!obs.IsValid())
        {
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            OrderModel.AddObservations(obs, orderToken, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("noAuth/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<GetFilteredEmailResponseNoAuth> GetToValidateEmailNoAuth(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string authToken = this.Request.Headers["Authorization"];
        string executer_user = "Anonymous";
        if (!string.IsNullOrEmpty(authToken))
        {
            executer_user = UserModel.GetUserByToken(authToken);
        }
        try
        {
            OrderDTONoAuth? order = OrderModel.GetOrderDTOByTokenNoAuth(orderToken, executer_user);
            if (order == null)
            {
                return new GetFilteredEmailResponseNoAuth(ResponseErrorMessage.NotFound, language);
            }

            FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executer_user, order.email_token, true);

            if (filteredEmail.IsEmpty())
            {
                return new GetFilteredEmailResponseNoAuth(null, null, order, ResponseSuccessMessage.Success, language);
            }

            List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, filteredEmail.email.id);

            return new GetFilteredEmailResponseNoAuth(filteredEmail, attachmentItems, order, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetToValidateEmail endpoint - Error - " + e);
            return new GetFilteredEmailResponseNoAuth(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("empty")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GetOrderDtoResponse> CreateEmptyOrder(CreateEmptyOrderRequest req)
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
            OrderDTO createdOrder = OrderModel.CreateEmptyOrder(null, req.is_draft, executer_user);

            return new GetOrderDtoResponse(createdOrder, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GetOrderDtoResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("validate")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<GetOrderDtoListResponse>> GetToValidateOrders([FromQuery] bool? is_draft, [FromQuery] bool? is_pending_approval, [FromQuery] bool? is_pending_credit, [FromQuery] DateOnly start_date, [FromQuery] DateOnly end_date)
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
            List<OrderDTO> orders = await OrderModel.GetToValidateOperatorPendingOrders(executer_user, is_draft, start_date, end_date, is_pending_approval, is_pending_credit);
            return new GetOrderDtoListResponse(orders, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("getToValidateOrders endpoint - Error - " + e);
            return new GetOrderDtoListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("audit")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<GetOrderDtoListResponse>> GetAuditOrders([FromQuery] bool is_draft, [FromQuery] DateOnly start_date, [FromQuery] DateOnly end_date)
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
            List<OrderDTO> orders = await OrderModel.GetAuditOrders(executer_user, [], is_draft, start_date, end_date);
            return new GetOrderDtoListResponse(orders, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetAuditOrders endpoint - Error - " + e);
            return new GetOrderDtoListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("validate/pending-client")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<GetOrderDtoListResponse>> GetToValidatePendingClients([FromQuery] DateOnly start_date, [FromQuery] DateOnly end_date)
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
            List<OrderDTO> orders = await OrderModel.GetToValidateClientPendingOrders(executer_user, start_date, end_date);
            return new GetOrderDtoListResponse(orders, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetToValidatePendingClients endpoint - Error - " + e);
            return new GetOrderDtoListResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<GetOrderDtoResponse> GetOrderByToken(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string authToken = this.Request.Headers["Authorization"];
        string executer_user = "Anonymous";
        if (!string.IsNullOrEmpty(authToken))
        {
            executer_user = UserModel.GetUserByToken(authToken);
        }
        try
        {
            OrderDTO? order = OrderModel.GetOrderDTOByToken(orderToken, executer_user);
            if (order is null)
            {
                return new GetOrderDtoResponse(ResponseErrorMessage.NotFound, language);
            }

            return new GetOrderDtoResponse(order, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetToValidateEmail endpoint - Error - " + e);
            return new GetOrderDtoResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpGet]
    [Route("{orderToken}/validate")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<GetOrderToValidateResponse> GetToValidateOrder(string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }
        string authToken = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(authToken);

        try
        {
            OrderDTO? order = OrderModel.GetOrderDTOByToken(orderToken, executer_user);
            if (order is null)
            {
                return new GetOrderToValidateResponse(ResponseErrorMessage.NotFound, language);
            }

            if (string.IsNullOrEmpty(order.email_token))
            {
                return new GetOrderToValidateResponse(null, null, order, ResponseSuccessMessage.Success, language);
            }

            FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(executer_user, order.email_token, true);

            if (filteredEmail.IsEmpty())
            {
                return new GetOrderToValidateResponse(null, null, order, ResponseSuccessMessage.Success, language);
            }

            List<EmailAttachmentItem> attachmentItems = AttachmentModel.getAttachments(executer_user, filteredEmail.email.id);

            return new GetOrderToValidateResponse(filteredEmail, attachmentItems, order, ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("GetToValidateEmail endpoint - Error - " + e);
            return new GetOrderToValidateResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{orderToken}/status")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> PatchOrderStatus(ChangeOrderStatusRequest req, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!req.IsValid())
        {
            return new GenericResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];
        string executer_user = UserModel.GetUserByToken(token);

        try
        {
            OrderModel.PatchStatus(orderToken, req.status_id, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPatch]
    [Route("{orderToken}/toggle-resolved")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public ActionResult<GenericResponse> ToggleOrderResolvedStatus(string orderToken)
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
            OrderModel.ToggleResolvedStatus(orderToken, executer_user);

            return new GenericResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }

    [HttpPost]
    [Route("invoice/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    [Authorize]
    public async Task<ActionResult<GenericResponse>> GenerateOrderInvoice(string orderToken)
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
            await OrderPrimaveraDocumentModel.CreateOrderDocuments(orderToken, executer_user);

            return new OrderPrimaveraDocumentResponse(ResponseSuccessMessage.Success, language);
        }
        catch (Exception e)
        {
            Log.Error("Filtered endpoint - Error - " + e);
            return new GenericResponse(ResponseErrorMessage.InternalError, language);
        }
    }
}
