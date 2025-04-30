// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class PrimaveraOrderModel
{
    public async static Task<List<PrimaveraOrderItem>> GetClientOrdersPrimaveraLastYear(string clientCode) => await GetClientOrdersPrimavera(clientCode, DateTime.Now.AddMonths(-12), null);
    public async static Task<List<PrimaveraOrderItem>> GetClientOrdersPrimaveraLastMonth(string clientCode) => await GetClientOrdersPrimavera(clientCode, DateTime.Now.AddMonths(-1), null);
    public async static Task<List<PrimaveraOrderItem>> GetPendingClientOrdersPrimavera(string clientCode) => await GetClientOrdersPrimavera(clientCode, null, PrimaveraEnums.EstadoEncomenda.PENDENTE);
    public async static Task<List<PrimaveraOrderItem>> GetPendingClientOrdersPrimaveraLastYear(string clientCode) => await GetClientOrdersPrimavera(clientCode, DateTime.Now.AddMonths(-12), PrimaveraEnums.EstadoEncomenda.PENDENTE);
    public async static Task<List<PrimaveraOrderItem>> GetClientOrdersPrimavera(string clientCode, DateTime? startDate, PrimaveraEnums.EstadoEncomenda? status)
    {
        // Get order headers
        List<PrimaveraOrderHeaderItem> primaveraClientOrdersHeaders = await GetClientOrderHeaders(clientCode, startDate, status);

        if (primaveraClientOrdersHeaders.Count == 0)
        {
            return [];
        }

        // Get order lines
        List<PrimaveraOrderLineItem> primaveraClientOrdersLines = await GetOrderLinesForHeaders(primaveraClientOrdersHeaders);

        // Process and return the orders
        return CreateOrderItemsFromHeadersAndLines(primaveraClientOrdersHeaders, primaveraClientOrdersLines);
    }

    private async static Task<List<PrimaveraOrderHeaderItem>> GetClientOrderHeaders(string clientCode, DateTime? startDate, PrimaveraEnums.EstadoEncomenda? status)
    {

        // Build the filter with client code and date
        string filter = $"Entidade=\'\'{clientCode}\'\' ";

        if (startDate.HasValue)
        {
            string startDateStr = startDate.Value.ToString("yyyy-MM-dd");
            filter += $" AND DataDoc >= \'\'{startDateStr}\'\' ";
        }

        // Add status filter if specified
        if (status.HasValue)
        {
            filter += $" AND Estado=\'\'{(char)status.Value}\'\' ";
        }

        PrimaveraListResponseItem<PrimaveraOrderHeaderItem> response = await Primavera.GetListAsync<PrimaveraOrderHeaderItem>(
            ConfigManager.PrimaveraUrls.EncomendasCabecalhos,
            999999,
            0,
            filter
        );

        if (response.IsError)
        {
            throw new PrimaveraApiErrorException(response.Message!);
        }

        return response.Data;
    }

    private async static Task<List<PrimaveraOrderLineItem>> GetOrderLinesForHeaders(List<PrimaveraOrderHeaderItem> headers)
    {
        // Limit the amount of lines we get per request to not get blocked
        int batchSize = 30;
        List<PrimaveraOrderLineItem> orderLines = [];

        for (int i = 0; i < headers.Count; i += batchSize)
        {
            List<string> batchIds = headers.Skip(i).Take(batchSize)
                .Select(x => $"IdCabec = \'\'{x.Id}\'\'")
                .ToList();

            string headerIds = string.Join(" OR ", batchIds);
            PrimaveraListResponseItem<PrimaveraOrderLineItem> batchOrderLines = await Primavera.GetListAsync<PrimaveraOrderLineItem>(
                ConfigManager.PrimaveraUrls.EncomendasLinhas,
                999999,
                0,
                headerIds
            );

            if (batchOrderLines.IsError)
            {
                throw new PrimaveraApiErrorException(batchOrderLines.Message!);
            }

            orderLines.AddRange(batchOrderLines.Data);
        }

        // Remove the lines that don't contain products, like documents and random stuff
        return orderLines.Where(x => x.Quantidade > 0).ToList();
    }

    private static List<PrimaveraOrderItem> CreateOrderItemsFromHeadersAndLines(
        List<PrimaveraOrderHeaderItem> headers,
        List<PrimaveraOrderLineItem> lines)
    {
        // Hash the orders by IdCabec for quick lookup
        Dictionary<string, List<PrimaveraOrderLineItem>> ordersLines = GroupLinesByHeader(lines);

        // Initialize the final list of orders
        List<PrimaveraOrderItem> clientOrders = [];
        foreach (PrimaveraOrderHeaderItem header in headers)
        {
            // Create a PrimaveraOrderItem for the current header and its lines
            PrimaveraOrderItem orderItem = new()
            {
                primavera_order_header = header,
                primavera_order_line = ordersLines.TryGetValue(header.Id, out var headerLines) ? headerLines : []
            };

            clientOrders.Add(orderItem);
        }

        // Sort the orders by descending date, so most recent first
        clientOrders.Sort((x, y) => y.primavera_order_header.DataDoc.CompareTo(x.primavera_order_header.DataDoc));

        return clientOrders;
    }

    private static Dictionary<string, List<PrimaveraOrderLineItem>> GroupLinesByHeader(List<PrimaveraOrderLineItem> lines)
    {
        Dictionary<string, List<PrimaveraOrderLineItem>> ordersLines = [];
        foreach (PrimaveraOrderLineItem line in lines)
        {
            if (!ordersLines.TryGetValue(line.IdCabec, out List<PrimaveraOrderLineItem>? existingLines))
            {
                existingLines = [];
                ordersLines[line.IdCabec] = existingLines;
            }
            existingLines.Add(line);
        }
        return ordersLines;
    }

    public async static Task<List<PrimaveraOrderHeaderItem>> GetAllOrdersHeaders()
    {
        string oneYearAgo = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");

        PrimaveraListResponseItem<PrimaveraOrderHeaderItem> primaveraClientOrdersHeaders = await Primavera.GetListAsync<PrimaveraOrderHeaderItem>(
            ConfigManager.PrimaveraUrls.EncomendasCabecalhos,
            999999,
            0,
            $"DataDoc >= \'\'{oneYearAgo}\'\'"
        );

        if (primaveraClientOrdersHeaders.IsError)
        {
            throw new PrimaveraApiErrorException(primaveraClientOrdersHeaders.Message!);
        }

        // Primavera, even though it might throw an error and not return anything, an
        // object on the list is always created, so we must check for 1
        if (primaveraClientOrdersHeaders.Data.Count <= 1)
        {
            throw new ResourceEmptyException("No orders on primavera found");
        }

        return primaveraClientOrdersHeaders.Data;
    }

    public async static Task<Dictionary<string, List<PrimaveraOrderHeaderItem>>> GetPrimaveraOrderHeadersByClientCodeHashed()
    {
        // Get all orders total from last year from primavera, just so we get the total and then compare each client volume with it
        List<PrimaveraOrderHeaderItem> orderHeaders = await GetAllOrdersHeaders();

        // Hash the orders by client code
        Dictionary<string, List<PrimaveraOrderHeaderItem>> ordersByClientCode = [];
        orderHeaders.ForEach((order) =>
        {
            if (string.IsNullOrEmpty(order.Entidade))
            {
                return;
            }

            if (!ordersByClientCode.TryGetValue(order.Entidade, out List<PrimaveraOrderHeaderItem>? clientOrders))
            {
                clientOrders = [];
                ordersByClientCode[order.Entidade] = clientOrders;
            }

            clientOrders.Add(order);
        });

        return ordersByClientCode;
    }

    public static decimal GetOrdersTotal(List<PrimaveraOrderHeaderItem>? headers)
    {
        if (headers == null)
        {
            return 0;
        }

        if (headers.Count <= 0)
        {
            return 0;
        }

        decimal total = 0;
        foreach (PrimaveraOrderHeaderItem header in headers)
        {
            total += header.TotalDocumento;
        }
        return total;
    }

    public async static Task<decimal> GetAllOrdersTotal()
    {
        string oneYearAgo = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");

        PrimaveraListResponseItem<PrimaveraOrderHeaderItem> primaveraClientOrdersHeaders = await Primavera.GetListAsync<PrimaveraOrderHeaderItem>(
            ConfigManager.PrimaveraUrls.EncomendasCabecalhos,
            999999,
            0,
            $"DataDoc >= \'\'{oneYearAgo}\'\'"
        );

        if (primaveraClientOrdersHeaders.IsError)
        {
            throw new PrimaveraApiErrorException(primaveraClientOrdersHeaders.Message!);
        }

        if (primaveraClientOrdersHeaders.Data.Count == 0)
        {
            throw new ResourceEmptyException("No orders on primavera found");
        }

        decimal total = GetOrdersTotal(primaveraClientOrdersHeaders.Data);
        return total;
    }
}
