// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.CTT;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using engimatrix.Views;
using Engimatrix.ModelObjs;
using Engimatrix.Models;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Z_Test;
using Microsoft.Graph.Models;

namespace engimatrix.Models;

public static class OrderModel
{
    // Get
    public static OrderItem? GetOrderByEmailToken(string email_token, string execute_user)
    {
        bool emailExists = FilteringModel.EmailTokenExists(email_token, execute_user);
        if (!emailExists)
        {
            throw new InputNotValidException("Email not found with token " + email_token);
        }

        Dictionary<string, string> dic = new()
        {
            { "emailToken", email_token }
        };

        string query = "SELECT id, token, email_token, postal_code, address, " +
            "city, district, locality, is_draft, cancel_reason_id, canceled_by, canceled_at " +
            "FROM `order` " +
            "WHERE `email_token` = @emailToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong getting the order from the database");
        }

        if (response.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> data = response.out_data[0];

        int cancelReasonId = string.IsNullOrEmpty(data["cancel_reason_id"]) ? 0 : Convert.ToInt32(data["cancel_reason_id"]);
        DateTime canceledAt = string.IsNullOrEmpty(data["canceled_at"]) ? DateTime.MinValue : Convert.ToDateTime(data["canceled_at"]);

        OrderItem order = new OrderItemBuilder()
            .SetId(Convert.ToInt32(data["id"]))
            .SetToken(data["token"])
            .SetEmailToken(data["email_token"])
            .SetPostalCode(data["postal_code"])
            .SetAddress(data["address"])
            .SetCity(data["city"])
            .SetDistrict(data["district"])
            .SetLocality(data["locality"])
            .SetIsDraft(Convert.ToBoolean(data["is_draft"]))
            .SetCancelReasonId(cancelReasonId)
            .SetCanceledBy(data["canceled_by"])
            .SetCanceledAt(canceledAt)
            .Build();

        if (ConfigManager.isProduction)
        {
            order.postal_code = Cryptography.Decrypt(order.postal_code, order.token);
            order.address = Cryptography.Decrypt(order.address, order.token);
            order.city = Cryptography.Decrypt(order.city, order.token);
            order.district = Cryptography.Decrypt(order.district, order.token);
            order.locality = Cryptography.Decrypt(order.locality, order.token);
        }

        return order;
    }

    // get by orderToken
    public static OrderItem? GetOrderByToken(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>
        {
            { "@orderToken", orderToken }
        };

        string query = "SELECT id, token, email_token, is_delivery, postal_code, address, " +
            "city, district, locality, client_code, is_draft, cancel_reason_id, canceled_by, canceled_at, " +
            "confirmed_by, confirmed_at, transport_id, maps_address, distance, travel_time, " +
            "observations, contact " +
            "FROM `order` " +
            "WHERE `token` = @orderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong getting the order from the database");
        }

        if (response.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> data = response.out_data[0];

        int cancelReasonId = string.IsNullOrEmpty(data["cancel_reason_id"]) ? 0 : Convert.ToInt32(data["cancel_reason_id"]);
        DateTime? canceledAt = string.IsNullOrEmpty(data["canceled_at"]) ? null : Convert.ToDateTime(data["canceled_at"]);
        DateTime? confirmedAt = string.IsNullOrEmpty(data["confirmed_at"]) ? null : Convert.ToDateTime(data["confirmed_at"]);
        int transportId = string.IsNullOrEmpty(data["transport_id"]) ? 0 : Convert.ToInt32(data["transport_id"]);
        int distance = string.IsNullOrEmpty(data["distance"]) ? 0 : Convert.ToInt32(data["distance"]);
        int travelTime = string.IsNullOrEmpty(data["travel_time"]) ? 0 : Convert.ToInt32(data["travel_time"]);

        OrderItem order = new OrderItemBuilder()
            .SetId(Convert.ToInt32(data["id"]))
            .SetToken(data["token"])
            .SetEmailToken(data["email_token"])
            .SetIsDelivery(Convert.ToBoolean(data["is_delivery"]))
            .SetPostalCode(data["postal_code"])
            .SetAddress(data["address"])
            .SetCity(data["city"])
            .SetDistrict(data["district"])
            .SetLocality(data["locality"])
            .SetClientCode(data["client_code"])
            .SetIsDraft(Convert.ToBoolean(data["is_draft"]))
            .SetCancelReasonId(cancelReasonId)
            .SetCanceledBy(data["canceled_by"])
            .SetCanceledAt(canceledAt)
            .SetConfirmedBy(data["confirmed_by"])
            .SetConfirmedAt(confirmedAt)
            .SetTransportId(transportId)
            .SetMapsAddress(data["maps_address"])
            .SetDistance(distance)
            .SetTravelTime(travelTime)
            .SetObservations(data["observations"])
            .SetContact(data["contact"])
            .Build();

        if (ConfigManager.isProduction)
        {
            order.postal_code = Cryptography.Decrypt(order.postal_code, orderToken);
            order.address = Cryptography.Decrypt(order.address, orderToken);
            order.city = Cryptography.Decrypt(order.city, orderToken);
            order.district = Cryptography.Decrypt(order.district, orderToken);
            order.locality = Cryptography.Decrypt(order.locality, orderToken);
        }

        return order;
    }

    /// <summary>
    /// Get OrderDTO by email token, and create it if email status is not A_PROCESSAR
    /// </summary>
    /// <param name="email_token"></param>
    /// <param name="execute_user"></param>
    /// <returns></returns>
    /// <exception cref="InputNotValidException"></exception>
    /// <exception cref="DatabaseException"></exception>
    private static OrderDTO? GetOrderDTO(string tokenValue, OrderConstants.TokenType tokenColumnType, string executeUser)
    {
        string tokenColumn = OrderConstants.GetColumnName(tokenColumnType);

        // Build parameters dictionary
        Dictionary<string, string> dic = new();
        dic.Add("tokenValue", tokenValue);

        string query = "SELECT o.id, o.is_delivery, o.token, o.email_token, o.postal_code, o.address, o.city, o.is_draft, " +
            "o.district, o.locality, o.client_nif, " +
            "cr.id AS cancel_reason_id, cr.reason AS cancel_reason, cr.slug AS cancel_slug, cr.description AS cancel_description, cr.is_active AS cancel_is_active, " +
            "o.canceled_by, o.canceled_at, o.client_code AS client_code, c.token AS client_token, " +
            "o.confirmed_by, o.confirmed_at, o.transport_id, t.name AS transport_name, " +
            "t.slug AS transport_slug, t.description AS transport_description, " +
            "o.maps_address as maps_address, o.distance AS distance, o.travel_time AS travel_time, " +
            "o.observations, o.contact, o.is_adjudicated, o.status_id, s.description AS status_description, " +
            "o.created_at, o.created_by " +
            "FROM `order` o " +
            "LEFT JOIN cancel_reason cr ON cr.id = o.cancel_reason_id " +
            "LEFT JOIN transport t ON t.id = o.transport_id " +
            "LEFT JOIN mf_client c ON c.code = o.client_code " +
            "JOIN status s ON s.id = o.status_id " +
            $"WHERE o.{tokenColumn} = @tokenValue";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetOrderRecord");

        if (!response.operationResult)
        {
            throw new DatabaseException("Something went wrong getting the order from the database");
        }

        if (response.out_data.Count == 0)
        {
            // For email tokens, you might want to attempt creating the order
            return tokenColumnType == OrderConstants.TokenType.EmailToken
                       ? VerifyEmailAndCreateOrder(tokenValue, executeUser)
                       : null;
        }

        Dictionary<string, string> data = response.out_data[0];

        List<OrderProductDTO> orderProducts = OrderProductModel.GetOrderProductsDTO(data["token"], executeUser);

        int cancelReasonId = string.IsNullOrEmpty(data["cancel_reason_id"]) ? 0 : Convert.ToInt32(data["cancel_reason_id"]);
        CancelReasonItem? cancelReason = null;

        // Create cancel reason DTO if cancelReasonId exists
        if (cancelReasonId != 0)
        {
            bool cancelIsActive = string.IsNullOrEmpty(data["cancel_is_active"]) ? false : Convert.ToInt32(data["cancel_is_active"]) != 0;

            cancelReason = new CancelReasonItemBuilder()
                .SetId(cancelReasonId)
                .SetReason(data["cancel_reason"])
                .SetSlug(data["cancel_slug"])
                .SetDescription(data["cancel_description"])
                .SetIsActive(cancelIsActive)
                .Build();
        }

        int transportId = string.IsNullOrEmpty(data["transport_id"]) ? 0 : Convert.ToInt32(data["transport_id"]);
        TransportItem? transport = null;

        if (transportId != 0)
        {
            transport = new TransportItemBuilder()
                .SetId(transportId)
                .SetName(data["transport_name"])
                .SetSlug(data["transport_slug"])
                .SetDescription(data["transport_description"])
                .Build();
        }

        DateTime? canceledAt = string.IsNullOrEmpty(data["canceled_at"]) ? null : Convert.ToDateTime(data["canceled_at"]);
        DateTime? confirmedAt = string.IsNullOrEmpty(data["confirmed_at"]) ? null : Convert.ToDateTime(data["confirmed_at"]);
        DateTime createdAt = string.IsNullOrEmpty(data["created_at"]) ? DateTime.MinValue : Convert.ToDateTime(data["created_at"]);

        // Helper function to handle empty strings as null
        static string? GetNullableString(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
        string? postalCode = GetNullableString(data["postal_code"]);
        string? address = GetNullableString(data["address"]);
        string? city = GetNullableString(data["city"]);
        string? district = GetNullableString(data["district"]);
        string? locality = GetNullableString(data["locality"]);
        string? canceledBy = GetNullableString(data["canceled_by"]);
        string? confirmedBy = GetNullableString(data["confirmed_by"]);
        string? mapsAddress = GetNullableString(data["maps_address"]);
        string? observations = GetNullableString(data["observations"]);
        string? contact = GetNullableString(data["contact"]);
        string? createdBy = GetNullableString(data["created_by"]);
        int? clientNif = string.IsNullOrEmpty(data["client_nif"]) ? null : Convert.ToInt32(data["client_nif"]);
        int? distance = string.IsNullOrEmpty(data["distance"]) ? null : Convert.ToInt32(data["distance"]);
        int? travelTime = string.IsNullOrEmpty(data["travel_time"]) ? null : Convert.ToInt32(data["travel_time"]);
        string orderToken = data["token"];
        bool isAdjudicated = Convert.ToBoolean(data["is_adjudicated"]);
        OrderTotalItem orderTotal = CalculateOrderTotal(orderToken, executeUser);

        List<OrderRatingDTO> orderRatings = OrderRatingModel.GetOrderRatingsByOrderTokenDTO(data["token"], executeUser);
        List<OrderRatingChangeRequestDto> orderRatingChangeRequestDtos = OrderRatingChangeRequestModel.GetDtoByOrderToken(data["token"], executeUser);

        OrderDTOBuilder orderBuilder = new OrderDTOBuilder()
            .SetId(Convert.ToInt32(data["id"]))
            .SetToken(data["token"])
            .SetEmailToken(data["email_token"])
            .SetIsDelivery(Convert.ToBoolean(data["is_delivery"]))
            .SetPostalCode(postalCode)
            .SetAddress(address)
            .SetCity(city)
            .SetDistrict(district)
            .SetLocality(locality)
            .SetIsDraft(Convert.ToBoolean(data["is_draft"]))
            .SetCancelReason(cancelReason)
            .SetCanceledBy(canceledBy)
            .SetCanceledAt(canceledAt)
            .SetConfirmedBy(confirmedBy)
            .SetConfirmedAt(confirmedAt)
            .SetTransport(transport)
            .SetClientNif(clientNif)
            .SetProducts(orderProducts)
            .SetRatings(orderRatings)
            .SetRatingChangeRequests(orderRatingChangeRequestDtos)
            .SetOrderTotal(orderTotal)
            .SetMapsAddress(mapsAddress)
            .SetDistance(distance)
            .SetTravelTime(travelTime)
            .SetObservations(observations)
            .SetContact(contact)
            .SetCreatedBy(createdBy)
            .SetCreatedAt(createdAt)
            .SetIsAdjudicated(isAdjudicated);

        StatusItem status = new(Convert.ToInt32(data["status_id"]), data["status_description"]);
        orderBuilder.SetStatus(status);

        // Sending the full client takes too long. So we just fill in the client code if it exists, so that the frontend
        // Can do a separate request for the client, and not have to wait for the order to load.
        if (!string.IsNullOrEmpty(data["client_code"]))
        {
            ClientDTO client = new ClientDTOBuilder().SetCode(data["client_code"]).SetToken(data["client_token"]).Build();
            orderBuilder.SetClient(client);
        }

        OrderDTO order = orderBuilder.Build();

        if (ConfigManager.isProduction)
        {
            order.postal_code = Cryptography.Decrypt(order.postal_code, order.token);
            order.address = Cryptography.Decrypt(order.address, order.token);
            order.city = Cryptography.Decrypt(order.city, order.token);
            order.district = Cryptography.Decrypt(order.district, order.token);
            order.locality = Cryptography.Decrypt(order.locality, order.token);
            order.observations = Cryptography.Decrypt(order.observations, order.token);
            order.contact = Cryptography.Decrypt(order.contact, order.token);
            order.maps_address = Cryptography.Decrypt(order.maps_address, order.token);
        }

        AddressFillingDetails? addressDetails = ExtractMunicipalityAndDistrictFromPostalCode(order.postal_code, executeUser);
        order.municipality_cc = addressDetails?.municipality_cc;
        order.district_dd = addressDetails?.district_dd;

        return orderBuilder.Build();
    }

    // Public method to get order by email token, including email validation.
    public static OrderDTO? GetOrderDTOByEmailToken(string emailToken, string executeUser)
    {
        bool emailExists = FilteringModel.EmailTokenExists(emailToken, executeUser);
        if (!emailExists)
        {
            throw new InputNotValidException("Email not found with token " + emailToken);
        }
        return GetOrderDTO(emailToken, OrderConstants.TokenType.EmailToken, executeUser);
    }

    // Public method to get order by order token.
    public static OrderDTO? GetOrderDTOByToken(string orderToken, string executeUser)
    {
        return GetOrderDTO(orderToken, OrderConstants.TokenType.OrderToken, executeUser);
    }

    public static OrderDTONoAuth? GetOrderDTOByTokenNoAuth(string token, OrderConstants.TokenType tokenType, string execute_user)
    {
        OrderDTO? order = GetOrderDTO(token, tokenType, execute_user);
        if (order == null)
        {
            return null;
        }

        ClientNoAuthDTO? client = new()
        {
            code = order.client?.code ?? "",
            token = order.client?.token ?? "",
            segment = order.client?.segment ?? new SegmentItem()
        };

        List<OrderProductDTONoAuth> products = [.. order.products.Select(p => new OrderProductDTONoAuth
        {
            id = p.id,
            order_token = p.order_token,
            quantity = p.quantity,
            confidence = p.confidence,
            is_instant_match = p.is_instant_match,
            is_manual_insert = p.is_manual_insert,
            product_catalog = new ProductCatalogDTONoAuth
            {
                id = p.product_catalog.id,
                product_code = p.product_catalog.product_code,
                description = p.product_catalog.description,
                description_full = p.product_catalog.description_full,
                type = p.product_catalog.type,
                shape = p.product_catalog.shape,
                material = p.product_catalog.material,
                finishing = p.product_catalog.finishing,
                surface = p.product_catalog.surface,
                length = p.product_catalog.length,
                width = p.product_catalog.width,
                height = p.product_catalog.height,
                diameter = p.product_catalog.diameter,
                unit = p.product_catalog.unit,
                family = p.product_catalog.family,
                product_conversions = p.product_catalog.product_conversions
            },
            product_unit = p.product_unit,
            rate_meters = p.rate_meters,
            price_discount = p.price_discount,
            calculated_price = p.calculated_price,
            price_locked_at = p.price_locked_at,
            is_price_locked = p.is_price_locked
        })];

        OrderDTONoAuth orderNoAuth = new()
        {
            id = order.id,
            token = order.token,
            email_token = order.email_token,
            is_delivery = order.is_delivery,
            postal_code = order.postal_code,
            address = order.address,
            city = order.city,
            district = order.district,
            locality = order.locality,
            is_draft = order.is_draft,
            cancel_reason = order.cancel_reason,
            municipality_cc = order.municipality_cc,
            district_dd = order.district_dd,
            transport = order.transport,
            order_total = order.order_total,
            products = products,
            client = client,
            is_adjudicated = order.is_adjudicated,
            status = order.status,
            created_at = order.created_at,
            created_by = order.created_by
        };

        return orderNoAuth;
    }

    public static OrderDTONoAuth? GetOrderDTOByTokenNoAuth(string orderToken, string execute_user)
    {
        OrderItem? orderItem = GetOrderByToken(orderToken, execute_user) ?? throw new InputNotValidException("GetOrderDTOByTokenNoAuth - Order not found with token " + orderToken);

        OrderDTONoAuth? order = GetOrderDTOByTokenNoAuth(orderItem.token, OrderConstants.TokenType.OrderToken, execute_user) ?? throw new InputNotValidException("GetOrderDTOByTokenNoAuth - Order DTO no auth found with email token " + orderItem.email_token);

        return order;
    }

    public static AddressFillingDetails? ExtractMunicipalityAndDistrictFromPostalCode(string? postalCode, string executeUser)
    {
        if (string.IsNullOrEmpty(postalCode))
        {
            Log.Debug("Postal code is null or empty");
            return null;
        }

        if (!postalCode.Contains('-'))
        {
            Log.Debug("Postal code does not contain a dash");
            return null;
        }

        string[] postalCodeParts = postalCode.Split("-");
        if (postalCodeParts.Length != 2)
        {
            Log.Debug("Postal code does not have 2 parts");
            return null;
        }

        string cp4 = postalCodeParts[0];
        string cp3 = postalCodeParts[1];

        if (cp4.Length != 4 || cp3.Length != 3)
        {
            Log.Debug("Postal code parts are not the correct length");
            return null;
        }

        List<CttPostalCodeItem> postalCodesItems = CttPostalCodeModel.GetAll(executeUser, null, null, cp4, cp3);
        if (postalCodesItems.Count <= 0)
        {
            Log.Debug("Postal code not found in database");
            return null;
        }

        CttPostalCodeItem postalCodeItem = postalCodesItems[0];

        string municipality_cc = postalCodeItem.cc;
        string district_dd = postalCodeItem.dd;

        return new AddressFillingDetails(municipality_cc, district_dd, cp4, cp3, postalCodeItem);
    }

    public static async Task<AddressFillingDetails?> GetClientAddressFillingDetails(string clientCode, string executeUser)
    {
        MFPrimaveraClientItem? client = await PrimaveraClientModel.GetPrimaveraClient(clientCode) ?? throw new InputNotValidException("Client not found with code " + clientCode);

        AddressFillingDetails? address = ExtractMunicipalityAndDistrictFromPostalCode(client?.CodPostal, executeUser);

        if (address is not null)
        {
            int transportId = string.IsNullOrEmpty(client!.Carro) ? 1 : Convert.ToInt32(client!.Carro);
            address.transport_id = transportId;
            address.address = client!.Morada ?? string.Empty;
            string? city = CttMunicipalityModel.GetByCcAndDd(address.municipality_cc, address.district_dd, executeUser)?.name;
            address.city = city ?? string.Empty;
        }

        return address;
    }

    // Insert
    public static void CreateOrder(OrderItem order, string execute_user)
    {
        if (ConfigManager.isProduction)
        {
            order.postal_code = Cryptography.Encrypt(order.postal_code, order.token);
            order.address = Cryptography.Encrypt(order.address, order.token);
            order.city = Cryptography.Encrypt(order.city, order.token);
            order.district = Cryptography.Encrypt(order.district, order.token);
            order.locality = Cryptography.Encrypt(order.locality, order.token);
            order.observations = Cryptography.Encrypt(order.observations, order.token);
            order.contact = Cryptography.Encrypt(order.contact, order.token);
            order.maps_address = Cryptography.Encrypt(order.maps_address, order.token);
        }

        Dictionary<string, string> dic = [];
        dic.Add("@token", order.token); dic.Add("@is_draft", order.is_draft.ToString());
        dic.Add("@is_delivery", order.is_delivery.ToString());
        dic.Add("@status_id", order.status_id.ToString());

        if (!string.IsNullOrEmpty(order.email_token))
        {
            dic.Add("@email_token", order.email_token);
        }
        if (!string.IsNullOrEmpty(order.address))
        {
            dic.Add("@address", order.address);
        }
        if (!string.IsNullOrEmpty(order.postal_code))
        {
            dic.Add("@postal_code", order.postal_code);
        }
        if (!string.IsNullOrEmpty(order.city))
        {
            dic.Add("@city", order.city);
        }
        if (!string.IsNullOrEmpty(order.district))
        {
            dic.Add("@district", order.district);
        }
        if (!string.IsNullOrEmpty(order.locality))
        {
            dic.Add("@locality", order.locality);
        }
        if (!string.IsNullOrEmpty(order.client_code))
        {
            dic.Add("@client_code", order.client_code);
        }
        if (!string.IsNullOrEmpty(order.created_by))
        {
            dic.Add("@created_by", order.created_by);
        }

        // Build the column names and values dynamically
        string columns = string.Join(", ", dic.Keys.Select(x => x[1..]));   // remove the @ symbol
        string values = string.Join(", ", dic.Keys);

        string query = $"INSERT INTO `order` ({columns}) VALUES ({values})";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "CreateOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong creating the order in the database");
        }

        // create default order ratings
        OrderRatingModel.CreateDefaultRatings(order.token, execute_user);
    }

    /// <summary>
    /// This method will be called when the user wants to access and order dto from an email that did not create it, because of some error.
    /// But, the order can be in a state where the email is still processing and the order is still yet to be created, so this is validated here
    /// </summary>
    /// <param name="emailToken"></param>
    /// <param name="execute_user"></param>
    /// <exception cref="InputNotValidException"></exception>
    /// <exception cref="EmailProcessingException"></exception>
    public static OrderDTO VerifyEmailAndCreateOrder(string emailToken, string execute_user)
    {
        // this method will be activated if the GetOrderDTO finds no order. This aims to check if the token provided
        // is for a valid email and, if it is, check its status. If status different than A_PROCESSAR, it means the order
        // that was supposed to be created, was not (If A_PROCESSAR, it means the email is currently awaiting the process for product extraction)
        FilteredEmail filteredEmail = FilteringModel.getFilteredEmail(execute_user, emailToken, true);

        bool isValid = IsFilteredEmailOrderValid(filteredEmail);
        if (!isValid)
        {
            throw new EmailProcessingException("Email is not valid for order creation");
        }

        bool isDraft = filteredEmail.category.Equals(CategoryConstants.CategoryTitle.COTACOES_ORCAMENTOS, StringComparison.OrdinalIgnoreCase);

        OrderDTO order = CreateEmptyOrder(emailToken, isDraft, execute_user);
        return order;
    }

    public static async Task<List<OrderDTO>> GetToValidateOperatorPendingOrders(string execute_user, bool? isDraft, DateOnly? startDate, DateOnly? endDate, bool? isPendingAdminApproval, bool? isPendingCreditApproval)
    {
        if (isPendingAdminApproval.HasValue && isPendingAdminApproval.Value)
        {
            return await GetToValidatePendingAdminApprovalOrders(execute_user, startDate, endDate);
        }

        if (isPendingCreditApproval.HasValue && isPendingCreditApproval.Value)
        {
            return await GetToValidatePendingCreditApprovalOrders(execute_user, startDate, endDate);
        }

        // the status can be the following:
        // Aguarda_Validacao, Pendente_Administracao, Erro, Aprovado_Direcao_Comercial
        List<string> statuses =
        [
            StatusConstants.StatusCode.ERRO.ToString(),
            StatusConstants.StatusCode.AGUARDA_VALIDACAO.ToString(),
            StatusConstants.StatusCode.PENDENTE_APROVACAO_ADMINISTRACAO.ToString(),
            StatusConstants.StatusCode.APROVADO_DIRECAO_COMERCIAL.ToString(),
            StatusConstants.StatusCode.PENDENTE_APROVACAO_CREDITO.ToString(),
        ];

        return await GetAuditOrders(execute_user, statuses, isDraft, startDate, endDate);
    }

    public static async Task<List<OrderDTO>> GetToValidatePendingAdminApprovalOrders(string execute_user, DateOnly? startDate, DateOnly? endDate)
    {
        List<string> statuses =
        [
            StatusConstants.StatusCode.PENDENTE_APROVACAO_ADMINISTRACAO.ToString(),
        ];

        return await GetAuditOrders(execute_user, statuses, null, startDate, endDate);
    }

    public static async Task<List<OrderDTO>> GetToValidatePendingCreditApprovalOrders(string execute_user, DateOnly? startDate, DateOnly? endDate)
    {
        List<string> statuses =
        [
            StatusConstants.StatusCode.PENDENTE_APROVACAO_CREDITO.ToString(),
        ];

        return await GetAuditOrders(execute_user, statuses, null, startDate, endDate);
    }

    public static async Task<List<OrderDTO>> GetToValidateClientPendingOrders(string execute_user, DateOnly? startDate, DateOnly? endDate)
    {
        List<string> statuses =
        [
            StatusConstants.StatusCode.PENDENTE_CONFIRMACAO_CLIENTE.ToString(),
        ];

        return await GetAuditOrders(execute_user, statuses, true, startDate, endDate);
    }

    public static async Task<List<OrderDTO>> GetAuditOrders(string execute_user, List<string> requiredStatuses, bool? isDraft, DateOnly? startDate, DateOnly? endDate)
    {
        Dictionary<string, string> dic = [];

        string query = "SELECT o.id AS order_id, o.token AS order_token, o.email_token AS order_email_token, " +
                "o.created_at, o.updated_at, o.is_draft, o.created_by, o.updated_by, o.client_code, o.is_adjudicated, " +
                "o.status_id AS status_id, s.description AS status_description, o.resolved_by, o.resolved_at " +
            "FROM `order` o " +
                "JOIN status s ON o.status_id = s.id " +
           "WHERE 1=1 ";

        // Dynamically add the statuses to the query and dictionary if provided
        if (requiredStatuses != null && requiredStatuses.Count > 0)
        {
            List<string> statusParams = new();
            for (int i = 0; i < requiredStatuses.Count; i++)
            {
                // Create a unique parameter name for each status
                string paramName = "@status" + i;
                statusParams.Add(paramName);
                // Add the parameter with the string representation of the status
                dic.Add(paramName, requiredStatuses[i].ToString());
            }
            query += "AND o.status_id IN (" + string.Join(", ", statusParams) + ") ";
        }

        if (isDraft.HasValue)
        {
            dic.Add("@isDraft", isDraft.Value.ToString());
            query += "AND o.is_draft = @isDraft ";
        }
        if (startDate.HasValue)
        {
            DateTime dateWithTime = startDate.Value.ToDateTime(new TimeOnly(0, 0, 0));
            dic.Add("startDate", dateWithTime.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND o.created_at >= @startDate ";
        }
        if (endDate.HasValue)
        {
            DateTime dateWithTime = endDate.Value.ToDateTime(new TimeOnly(23, 59, 59));
            dic.Add("endDate", dateWithTime.ToString("yyyy-MM-dd HH:mm:ss"));
            query += "AND o.created_at <= @endDate ";
        }

        query += "ORDER BY o.created_at DESC";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetFilteredEmails");

        if (!response.operationResult) { throw new DatabaseException("Something went wrong getting the order with the filtered emails from the database"); }

        if (response.out_data.Count == 0) { return []; }

        List<string> filteredEmailTokens = [.. response.out_data
            .Select(x => x["order_email_token"])
            .Where(token => !string.IsNullOrEmpty(token))
            .Distinct()];
        List<FilteredEmailDTONew> filteredEmails = await FilteringModel.GetFilteredDtoNew(filteredEmailTokens, execute_user);
        // Use GroupBy to handle duplicates - take first occurrence of each token
        Dictionary<string, FilteredEmailDTONew> filteredByToken = filteredEmails
            .GroupBy(x => x.token)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.email?.id ?? string.Empty).First()
            );

        List<OrderDTO> orders = [];
        foreach (Dictionary<string, string> data in response.out_data)
        {
            // build order status
            StatusItem status = new(Convert.ToInt32(data["status_id"]), data["status_description"]);

            DateTime? resolvedAt = string.IsNullOrEmpty(data["resolved_at"]) ? null : Convert.ToDateTime(data["resolved_at"]);

            // build order
            OrderDTOBuilder orderDtoBuilder = new OrderDTOBuilder()
                .SetId(Convert.ToInt32(data["order_id"]))
                .SetToken(data["order_token"])
                .SetEmailToken(data["order_email_token"])
                .SetStatus(status)
                .SetIsDraft(Convert.ToBoolean(data["is_draft"]))
                .SetCreatedAt(Convert.ToDateTime(data["created_at"]))
                .SetUpdatedAt(Convert.ToDateTime(data["updated_at"]))
                .SetCreatedBy(data["created_by"])
                .SetUpdatedBy(data["updated_by"])
                .SetResolvedAt(resolvedAt)
                .SetIsAdjudicated(Convert.ToBoolean(data["is_adjudicated"]))
                .SetResolvedBy(data["resolved_by"]);

            string clientCode = data["client_code"];
            if (!string.IsNullOrEmpty(clientCode))
            {
                MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);

                ClientDTOBuilder client = new ClientDTOBuilder()
                    .SetCode(clientCode);

                if (primaveraClient != null)
                {
                    client.SetPrimaveraClient(primaveraClient);
                }

                orderDtoBuilder.SetClient(client.Build());
            }

            // check if there is an email token on the order
            if (!string.IsNullOrEmpty(data["order_email_token"]))
            {
                if (!filteredByToken.TryGetValue(data["order_email_token"], out FilteredEmailDTONew? filteredEmail))
                {
                    Log.Debug($"Orders: Token not found! {data["order_email_token"]}");
                }

                orderDtoBuilder.SetFilteredEmail(filteredEmail);
            }

            OrderDTO order = orderDtoBuilder.Build();
            orders.Add(order);
        }

        return orders;
    }

    public static bool IsFilteredEmailOrderValid(FilteredEmail? filteredEmail)
    {
        if (filteredEmail == null || string.IsNullOrEmpty(filteredEmail.email.id) || string.IsNullOrEmpty(filteredEmail.token))
        {
            Log.Debug("Filtered email not found or missing required fields");
            return false;
        }

        string[] allowedCategories =
        [
            CategoryConstants.CategoryTitle.ENCOMENDAS,
            CategoryConstants.CategoryTitle.COTACOES_ORCAMENTOS
        ];

        if (!allowedCategories.Contains(filteredEmail.category))
        {
            Log.Debug("Email category is not ENCOMENDAS or COTACOES_ORCAMENTOS");
            return false;
        }

        string processingDescription = StatusConverter.Convert(StatusConstants.StatusCode.A_PROCESSAR);

        if (filteredEmail.status.Equals(processingDescription, StringComparison.OrdinalIgnoreCase))
        {
            Log.Debug("Email is currently in status A_PROCESSAR, so the order is not yet created");
            return false;
        }

        return true;
    }

    // update address
    public static void UpdateOrderAddress(string orderToken, OrderAddressRequest address, string execute_user)
    {
        bool orderExists = OrderExists(orderToken, execute_user);
        if (!orderExists)
        {
            throw new NotFoundException("Order not found with order token " + orderToken);
        }

        Dictionary<string, string> dic = [];
        dic.Add("@token", orderToken);
        dic.Add("@IsDelivery", address.is_delivery.ToString());

        string query = "UPDATE `order` " +
            "SET is_delivery = @IsDelivery ";

        if (address.is_delivery)
        {
            if (string.IsNullOrEmpty(address.postal_code_cp4) || string.IsNullOrEmpty(address.postal_code_cp3) || string.IsNullOrEmpty(address.address) || !address.transport_id.HasValue)
            {
                throw new InputNotValidException("Missing required fields for delivery address");
            }

            List<CttPostalCodeItem> postalCodeItem = CttPostalCodeModel.GetAll(execute_user, null, null, address.postal_code_cp4, address.postal_code_cp3);
            if (postalCodeItem.Count == 0)
            {
                throw new InputNotValidException($"Postal code not found with cp4: {address.postal_code_cp4} and cp3: {address.postal_code_cp3}");
            }

            CttMunicipalityItem? municipalityItem = CttMunicipalityModel.GetByCcAndDd(postalCodeItem[0].cc, postalCodeItem[0].dd, execute_user);
            if (municipalityItem == null)
            {
                throw new InputNotValidException($"Municipality not found with cc: {postalCodeItem[0].cc} and dd: {postalCodeItem[0].dd}");
            }

            CttDistrictItem? districtItem = CttDistrictModel.GetByDd(municipalityItem.dd, execute_user);
            if (districtItem == null)
            {
                throw new InputNotValidException($"District not found with dd: {municipalityItem.dd}");
            }

            string locality = postalCodeItem[0].cpalf;
            string postalCode = address.postal_code_cp4 + "-" + address.postal_code_cp3;
            string city = municipalityItem.name;
            string district = districtItem.name;

            if (ConfigManager.isProduction)
            {
                postalCode = Cryptography.Encrypt(postalCode, orderToken);
                city = Cryptography.Encrypt(city, orderToken);
                district = Cryptography.Encrypt(district, orderToken);
                locality = Cryptography.Encrypt(locality, orderToken);
                address.address = Cryptography.Encrypt(address.address, orderToken);
            }

            dic.Add("@city", city);
            dic.Add("@locality", locality);
            dic.Add("@district", district);
            dic.Add("@postalCode", postalCode);
            dic.Add("@address", address.address);
            dic.Add("@TransportId", address.transport_id.Value.ToString());
            query += ", postal_code = @postalCode, address = @address, city = @city, transport_id = @TransportId, " +
                "locality = @locality, district = @district ";
        }

        query += "WHERE token = @token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "UpdateOrderAddressRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong updating the order address in the database");
        }
    }

    public static void CleanOrderAddress(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@token", orderToken);

        string query = "UPDATE `order` " +
            "SET postal_code = NULL, address = NULL, city = NULL, transport_id = NULL, locality = NULL, district = NULL, " +
            "maps_address = NULL, distance = NULL, travel_time = NULL " +
            "WHERE token = @token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "CleanOrderAddressRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong cleaning the order address in the database");
        }
    }

    public static void PatchBigTransport(string orderToken, string executeUser)
    {
        OrderItem? order = GetOrderByToken(orderToken, executeUser) ?? throw new NotFoundException("Order not found with order token " + orderToken);
        if (order.transport_id != 3)
        {
            throw new Exception("Order does not have a big transport");
        }

        Dictionary<string, string> dic = new()
        {
            { "@orderToken", orderToken }
        };
        string query = "UPDATE `order` SET transport_id = 2 WHERE token = @orderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "PatchBigTransportRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong patching the big transport in the database");
        }

        Log.Debug($"Order {orderToken} has been patched to smaller transport by {executeUser}");
    }

    public static void UpdateOrderHereMapsAddress(string orderToken, HereRoutesItemResponse route, string executeUser)
    {
        ArgumentNullException.ThrowIfNull(route);

        if (route.Routes.Count == 0 || route.Routes[0].Sections.Count == 0)
        {
            throw new InputNotValidException("Route has no sections");
        }

        Section section = route.Routes[0].Sections[0];

        if (section.Summary == null || section.Summary.Length == 0 || section.Summary.Duration == 0)
        {
            throw new InputNotValidException("Route section summary is null");
        }

        string mapsAddress = GetDestinationCoordinates(route);
        int distance = section.Summary.Length;
        int travelTime = section.Summary.Duration;

        Dictionary<string, string> dic = [];
        if (ConfigManager.isProduction)
        {
            mapsAddress = Cryptography.Encrypt(mapsAddress, orderToken);
        }

        dic.Add("@MapsAddress", mapsAddress);
        dic.Add("@Distance", distance.ToString());
        dic.Add("@TravelTime", travelTime.ToString());
        dic.Add("@OrderToken", orderToken);

        string query = "UPDATE `order` " +
            "SET maps_address = @MapsAddress, distance = @Distance, travel_time = @TravelTime " +
            "WHERE token = @OrderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "UpdateOrderMapsAddressRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong updating the order maps address in the database");
        }
    }

    public static string GetDestinationCoordinates(HereRoutesItemResponse route)
    {
        ModelObjs.Place arrivalPlace = route.Routes[0].Sections[0].Arrival.Place;
        double lat = arrivalPlace.Location.Lat;
        double lng = arrivalPlace.Location.Lng;
        return lat + "," + lng;
    }

    // Cancel Order
    public static void CancelOrder(string orderToken, int cancelReasonId, string execute_user)
    {
        Dictionary<string, string> dic = [];
        dic.Add("@token", orderToken);
        dic.Add("@cancelReasonId", cancelReasonId.ToString());
        dic.Add("@canceledBy", execute_user);
        dic.Add("@canceledAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

        string query = "UPDATE `order` SET is_draft = 0, cancel_reason_id = @cancelReasonId, canceled_by = @canceledBy, canceled_at = @canceledAt " +
            "WHERE token = @token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "CancelOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong canceling the order in the database");
        }

        // get the filtered email associated with that order token
        OrderItem? order = GetOrderByToken(orderToken, execute_user);
        if (order == null)
        {
            throw new Exception("Order not found with order token " + orderToken);
        }

        int statusId = StatusConstants.StatusCode.CANCELADO_POR_OPERADOR;
        PatchStatus(orderToken, statusId, execute_user);
    }

    // client confirm order
    public static async Task ConfirmOrder(string orderToken, string execute_user)
    {
        OrderItem? order = GetOrderByToken(orderToken, execute_user) ?? throw new NotFoundException("Order not found with order token " + orderToken);
        if (order.confirmed_at != null)
        {
            throw new Exception("Order already confirmed");
        }

        // The following code is way less error prone so the must error prone thing has been done first
        Dictionary<string, string> dic = [];
        dic.Add("@Token", orderToken);
        dic.Add("@ConfirmedBy", execute_user);
        dic.Add("@ConfirmedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

        string query = "UPDATE `order` " +
            "SET confirmed_by = @ConfirmedBy, confirmed_at = @ConfirmedAt " +
            "WHERE token = @Token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "ConfirmOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong confirming the order in the database");
        }

        // check if the order has enough credit before proceding. If not enough credit, we must set the status to Pending Client Managment Approval
        if (!await ClientHasEnoughCredit(order, order.client_code, execute_user))
        {
            Log.Debug("ConfirmOrder: Client does not have enough credit for the order");
            // change the status to pending Client Management
            PatchStatus(orderToken, StatusConstants.StatusCode.PENDENTE_APROVACAO_CREDITO, execute_user);
            return;
        }

        // if it is an order, we just make it confirmed
        int statusId = StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR;
        if (execute_user.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
        {
            statusId = StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE;
        }
        // change the filtered email status to Confirmado por Cliente
        PatchStatus(orderToken, statusId, execute_user);
    }

    public static async Task<bool> ClientHasEnoughCredit(OrderItem order, string clientCode, string executeUser)
    {
        MFPrimaveraClientItem primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode) ?? throw new InputNotValidException("Client not found with code " + clientCode);

        if (string.IsNullOrEmpty(primaveraClient.PlafoundCesce))
        {
            // No plafound registered means no credit
            Log.Debug("ClientHasEnoughCredit: Client has credit null or empty");
            return false;
        }

        decimal plafound = Convert.ToDecimal(primaveraClient.PlafoundCesce);

        if (plafound <= 0)
        {
            Log.Debug("ClientHasEnoughCredit: Client has no credit, zero");
            return false;
        }

        MFPrimaveraInvoiceTotalItem invoicesTotal = new(50, 100, 200);
        OrderTotalItem orderTotal = CalculateOrderTotal(order.token, executeUser);

        decimal totalClientCredit = orderTotal.totalDiscountPlusTax + 1000 + (decimal)invoicesTotal.valor_pendente;

        if (totalClientCredit > plafound)
        {
            Log.Debug($"ClientHasEnoughtCredit: Client {clientCode} has not enough credit for the order. Total: {totalClientCredit}, Plafound: {plafound}");
            return false;
        }

        Log.Debug($"ClientHasEnoughtCredit: Client {clientCode} has enough credit for the order. Total: {totalClientCredit}, Plafound: {plafound}");
        return true;
    }

    public static async Task AdjudicateOrder(string orderToken, string executeUser)
    {
        Dictionary<string, string> dic = new()
        {
            { "@orderToken", orderToken }
        };
        string query = "UPDATE `order` SET is_adjudicated = 1 WHERE token = @orderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "AdjudicateOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong adjudicating the order in the database");
        }

        // verify if the order has enough credit to be converted to a real order
        OrderItem? order = GetOrderByToken(orderToken, executeUser) ?? throw new NotFoundException("Order not found with order token " + orderToken);

        // check if the order has enough credit before proceding. If not enough credit, we must set the status to Pending Client Managment Approval
        if (!await ClientHasEnoughCredit(order, order.client_code, executeUser))
        {
            Log.Debug("AdjudicateOrder: Client does not have enough credit for the order");
            // change the status to pending Client Management
            PatchStatus(orderToken, StatusConstants.StatusCode.PENDENTE_APROVACAO_CREDITO, executeUser);
            return;
        }

        // set status to confirmed
        int statusId = StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR;
        if (executeUser.Equals(string.Empty, StringComparison.OrdinalIgnoreCase)) { statusId = StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE; }

        PatchStatus(orderToken, statusId, executeUser);

        Log.Debug($"Order {orderToken} has been adjudicated by {executeUser} (if empty it was the client)");
    }

    // Get OrderDTO with their filteredEmail confirmed by operator or client
    public static List<OrderItem> GetOrdersConfirmedByOperatorOrClient(string execute_user)
    {
        // There are X ways for an order to be in its final state -> confirmed
        // If it was a quotation, it must be adjudicated by the client (is_adjudicated = 1)
        // if it was an order, it must be confirmed by the operator or the client
        string confirmedStatusList = $"{StatusConstants.StatusCode.CONFIRMADO_POR_OPERADOR}, {StatusConstants.StatusCode.CONFIRMADO_POR_CLIENTE}";

        string query = "SELECT o.id, o.token, o.email_token, o.is_delivery, o.postal_code, o.address, o.city, o.is_draft, o.status_id, " +
            "o.canceled_by, o.canceled_at " +
            "FROM `order` o " +
            // this Where means the order is a quotation, and after pending the client approval, he approved, converting the quotation in an order
            $"WHERE (o.is_draft = 1 AND o.status_id IN ({confirmedStatusList}) AND o.is_adjudicated = 1) " +
            // this is a direct order creation, and was confirmed by either the operator or the client
            $"OR (o.is_draft = 0 AND o.status_id IN ({confirmedStatusList}))";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetOrderRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong getting the order from the database");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<OrderItem> orders = [];
        foreach (Dictionary<string, string> data in response.out_data)
        {
            OrderItem order = new OrderItemBuilder()
                .SetId(Convert.ToInt32(data["id"]))
                .SetToken(data["token"])
                .SetEmailToken(data["email_token"])
                .SetIsDelivery(Convert.ToBoolean(data["is_delivery"]))
                .SetPostalCode(data["postal_code"])
                .SetAddress(data["address"])
                .SetCity(data["city"])
                .SetIsDraft(Convert.ToBoolean(data["is_draft"]))
                .SetCanceledBy(data["canceled_by"])
                .Build();

            if (ConfigManager.isProduction)
            {
                order.postal_code = Cryptography.Decrypt(order.postal_code, order.token);
                order.address = Cryptography.Decrypt(order.address, order.token);
                order.city = Cryptography.Decrypt(order.city, order.token);
                order.district = Cryptography.Decrypt(order.district, order.token);
                order.locality = Cryptography.Decrypt(order.locality, order.token);
                order.observations = Cryptography.Decrypt(order.observations, order.token);
                order.contact = Cryptography.Decrypt(order.contact, order.token);
                order.maps_address = Cryptography.Decrypt(order.maps_address, order.token);
            }

            orders.Add(order);
        }

        return orders;
    }

    public static async Task<(AddressFillingDetails? addressDetails, List<OrderProductItem> orderProducts)> PatchClient(string orderToken, string clientCode, string client_nif, string execute_user)
    {
        if (!Util.IsValidNif(client_nif))
        {
            throw new InputNotValidException("NIF is not valid");
        }

        MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);
        if (primaveraClient == null)
        {
            throw new InputNotValidException("Client not found with code " + clientCode);
        }

        // get the order
        OrderItem? order = GetOrderByToken(orderToken, execute_user);
        if (order == null)
        {
            throw new Exception("Order not found with order token " + orderToken);
        }

        if (order.confirmed_at != null)
        {
            throw new Exception("Cannot change client on confirmed order");
        }

        Dictionary<string, string> dic = [];
        dic.Add("@token", orderToken);
        dic.Add("@clientCode", clientCode);
        dic.Add("@clientNif", client_nif);

        string query = "UPDATE `order` SET client_code = @clientCode, client_nif = @clientNif WHERE token = @token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "EditClientRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong editing the client in the database");
        }

        //update the address
        AddressFillingDetails? address = await PatchClientAddress(order, clientCode, execute_user);

        // When changing the client, we want to update all associated product prices
        List<OrderProductItem> updatedProducts = RecalculateOrderProductsPrice(orderToken, execute_user);
        return (address, updatedProducts);
    }

    public static async Task<AddressFillingDetails?> PatchClientAddress(OrderItem order, string clientCode, string executeUser)
    {
        AddressFillingDetails? address = await GetClientAddressFillingDetails(clientCode, executeUser);
        if (address is null)
        {
            // clean the order address
            CleanOrderAddress(order.token, executeUser);
            return null;
        }

        OrderAddressRequest orderAddress = new()
        {
            is_delivery = order.is_delivery,
            postal_code_cp4 = address.postal_code.cp4,
            postal_code_cp3 = address.postal_code.cp3,
            address = address.address,
            transport_id = address.transport_id,
        };

        try
        {
            UpdateOrderAddress(order.token, orderAddress, executeUser);
        }
        catch (Exception e)
        {
            // clean the order address
            CleanOrderAddress(order.token, executeUser);
            Log.Error("PatchClientAddress - Error updating order address: " + e.Message);
            address = null;
        }

        return address;
    }

    public static OrderTotalItem CalculateOrderTotal(string orderToken, string executeUser)
    {
        List<OrderProductItem> products = OrderProductModel.GetOrderProducts(orderToken, executeUser);
        if (products.Count == 0)
        {
            return new OrderTotalItem(0, 0, 0);
        }

        // product catalogs
        List<string> productIds = [.. products.Select(x => x.product_catalog_id.ToString())];
        List<ProductCatalogItem> productCatalogs = ProductCatalogModel.GetProductCatalogsByIds(productIds, false, executeUser);
        Dictionary<int, ProductCatalogItem> productCatalogsMap = productCatalogs.ToDictionary(x => x.id);

        // conversions
        List<ProductConversionItem> productConversions = ProductConversionModel.GetProductConversions(executeUser);
        Dictionary<string, List<ProductConversionItem>> productConversionsMap = productConversions.GroupBy(x => x.product_code).ToDictionary(x => x.Key, x => x.ToList());

        List<ProductUnitItem> units = ProductUnitModel.GetProductUnits(executeUser);

        // The price of the product is on the product_catalogs default unit, so first we must convert it to the unit the product was ordered in
        decimal total = 0;
        decimal totalWithDiscount = 0;
        foreach (OrderProductItem product in products)
        {
            try
            {
                // Use the helper to get the quantity in the catalog's base unit.
                (decimal _, decimal quantityInBaseUnit) =
                    OrderProductModel.GetConvertedQuantities(
                        product, productCatalogsMap, productConversionsMap, units);

                // Use the quantity in base unit for pricing calculations
                decimal productPrice = Math.Round(product.calculated_price * quantityInBaseUnit, 2, MidpointRounding.AwayFromZero);
                total += productPrice;

                decimal productDiscountPrice = Math.Round(product.price_discount * quantityInBaseUnit, 2, MidpointRounding.AwayFromZero);
                totalWithDiscount += productDiscountPrice;
            }
            catch (NotFoundException ex)
            {
                Log.Debug("Conversion error for product " + product.product_catalog_id + ": " + ex.Message);
                continue;
            }
        }

        decimal totalRounded = Math.Round(total, 2, MidpointRounding.AwayFromZero);
        decimal totalWithDiscountRounded = Math.Round(totalWithDiscount, 2, MidpointRounding.AwayFromZero);

        decimal totalWithDiscountAndTax = totalWithDiscountRounded * 1.23m;
        decimal totalWithDiscountAndTaxRounded = Math.Round(totalWithDiscountAndTax, 2, MidpointRounding.AwayFromZero);

        OrderTotalItem orderTotal = new(totalRounded, totalWithDiscountRounded, totalWithDiscountAndTaxRounded);
        Log.Debug($"Order total with token {orderToken}: {orderTotal}");

        return orderTotal;
    }

    public static List<OrderProductItem> RecalculateOrderProductsPrice(string orderToken, string execute_user)
    {
        // get the products
        List<OrderProductItem> orderProducts = OrderProductModel.GetOrderProducts(orderToken, execute_user);
        if (orderProducts.Count == 0)
        {
            Log.Debug("No products found for order " + orderToken);
            return [];
        }

        List<OrderProductItem> updatedProducts = [];

        foreach (OrderProductItem product in orderProducts)
        {
            OrderProductItem updatedProduct = OrderProductModel.UpdateOrderProduct(product, execute_user);
            updatedProducts.Add(updatedProduct);
        }

        return updatedProducts;
    }

    public static OrderDTO CreateEmptyOrder(string? filteredEmailToken, bool isDraft, string execute_user)
    {
        OrderItemBuilder orderBuilder = new OrderItemBuilder()
            .SetToken(Guid.NewGuid().ToString())
            .SetIsDelivery(true)
            .SetIsDraft(isDraft)
            .SetEmailToken(filteredEmailToken)
            .SetCreatedBy(execute_user)
            .SetStatusId(StatusConstants.StatusCode.AGUARDA_VALIDACAO);

        OrderItem order = orderBuilder.Build();

        CreateOrder(order, execute_user);

        OrderDTO? orderDto = GetOrderDTOByToken(order.token, execute_user) ?? throw new Exception("Error creating empty order");

        return orderDto;
    }

    public static bool OrderExists(string orderToken, string execute_user)
    {
        Dictionary<string, string> dic = new(){
            { "@orderToken", orderToken }
        };

        string query = "SELECT COUNT(*) AS count FROM `order` WHERE token = @orderToken";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "CheckOrderExists");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong checking if the order exists in the database");
        }

        if (response.out_data.Count == 0)
        {
            return false;
        }

        return Convert.ToInt32(response.out_data[0]["count"]) > 0;
    }

    public static void AddObservations(OrderObservationsRequest obs, string orderToken, string executeUser)
    {
        string observations = obs.observations ?? string.Empty;
        string contact = obs.contact ?? string.Empty;

        // Encrypt only in production
        if (ConfigManager.isProduction)
        {
            observations = Cryptography.Encrypt(observations, orderToken);
            contact = Cryptography.Encrypt(contact, orderToken);
        }

        Dictionary<string, string> dic = new()
        {
            { "@token", orderToken },
            { "@observations", observations },
            { "@contact", contact }
        };

        string query = @"UPDATE `order`
            SET observations = @observations, contact = @contact
            WHERE token = @token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "AddObservationsRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong adding the observations to the order in the database");
        }
    }

    public static void PatchStatus(string orderToken, int statusId, string executeUser)
    {
        Dictionary<string, string> dic = new()
        {
            { "@token", orderToken },
            { "@statusId", statusId.ToString() }
        };

        string query = "UPDATE `order` SET status_id = @statusId ";

        if (statusId == StatusConstants.StatusCode.RESOLVIDO_MANUALMENTE)
        {
            dic.Add("@ResolvedBy", executeUser);
            dic.Add("@ResolvedAt", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            query += ", resolved_by = @ResolvedBy, resolved_at = @ResolvedAt ";
        }

        query += "WHERE token = @token";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "ChangeOrderStatusRecord");

        if (!response.operationResult)
        {
            throw new Exception("Something went wrong changing the order status in the database");
        }
    }
}
