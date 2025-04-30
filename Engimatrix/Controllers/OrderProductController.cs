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

namespace Engimatrix.Controllers;
[ApiController]
[Route("api/orders/products")]
public class OrderProductController : ControllerBase
{
    [HttpPut]
    [Route("{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<OrderProductUpdateResponse> UpdateOrderProductsList(OrderProductRequest productsReq, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!productsReq.IsValid())
        {
            return new OrderProductUpdateResponse(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];

        string executer_user = "Anonymous Client";
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<OrderProductItem> updatedProducts = OrderProductModel.UpdateOrderProducts(orderToken, productsReq.products, executer_user);

            // Get the operational cost rating for the order
            List<OrderRatingDTO> orderRatings = OrderRatingModel.GetOrderRatingsByOrderTokenDTO(orderToken, executer_user);

            OrderTotalItem orderTotal = OrderModel.CalculateOrderTotal(orderToken, executer_user);

            return new OrderProductUpdateResponse(updatedProducts, orderTotal, orderRatings, ResponseSuccessMessage.Success, language);
        }
        catch (ProductStockNotValidException e)
        {
            Log.Error($"UpdateProductList endpoint - Stock Not Valid Error - {e}");
            return new OrderProductUpdateResponse(ResponseErrorMessage.ProductInvalidStock, language);
        }
        catch (DatabaseException)
        {
            Log.Error("UpdateProductList endpoint - Error - Database error");
            return new OrderProductUpdateResponse(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateProductList endpoint - Error - " + e);
            return new OrderProductUpdateResponse(ResponseErrorMessage.InternalError, language);
        }
    }


    [HttpPut]
    [Route("no-auth/{orderToken}")]
    [RequestLimit]
    [ValidateReferrer]
    public ActionResult<OrderProductUpdateResponseNoAuth> UpdateOrderProductsListNoAuth(OrderProductRequest productsReq, string orderToken)
    {
        string language = this.Request.Headers["client-lang"];
        if (string.IsNullOrEmpty(language))
        {
            language = ConfigManager.defaultLanguage;
        }

        if (!productsReq.IsValid())
        {
            return new OrderProductUpdateResponseNoAuth(ResponseErrorMessage.InvalidArgs, language);
        }

        string token = this.Request.Headers["Authorization"];

        string executer_user = "Anonymous Client";
        if (!string.IsNullOrEmpty(token))
        {
            executer_user = UserModel.GetUserByToken(token);
        }

        try
        {
            List<OrderProductItem> updatedProducts = OrderProductModel.UpdateOrderProducts(orderToken, productsReq.products, executer_user);

            OrderTotalItem orderTotal = OrderModel.CalculateOrderTotal(orderToken, executer_user);

            return new OrderProductUpdateResponseNoAuth(updatedProducts, orderTotal, ResponseSuccessMessage.Success, language);
        }
        catch (ProductStockNotValidException e)
        {
            Log.Error($"UpdateProductList endpoint - Stock Not Valid Error - {e}");
            return new OrderProductUpdateResponseNoAuth(ResponseErrorMessage.ProductInvalidStock, language);
        }
        catch (DatabaseException)
        {
            Log.Error("UpdateProductList endpoint - Error - Database error");
            return new OrderProductUpdateResponseNoAuth(ResponseErrorMessage.DatabaseQueryError, language);
        }
        catch (Exception e)
        {
            Log.Error("UpdateProductList endpoint - Error - " + e);
            return new OrderProductUpdateResponseNoAuth(ResponseErrorMessage.InternalError, language);
        }
    }
}
