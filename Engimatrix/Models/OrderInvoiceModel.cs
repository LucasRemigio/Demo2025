// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using Engimatrix.ModelObjs;

namespace engimatrix.Models;

public static class OrderInvoiceModel
{
    // this basically grabs the template and changes the variables
    // for the correct values intended for the invoice
    public async static Task<string> GenerateInvoice(PrimaveraDocument primaveraDoc, string orderToken, string executeUser)
    {
        OrderItem? orderItem = OrderModel.GetOrderByToken(orderToken, executeUser) ?? throw new NotFoundException("Order not found");

        MFPrimaveraClientItem client = await PrimaveraClientModel.GetPrimaveraClient(orderItem.client_code) ?? throw new NotFoundException("Client not found");

        string templatePath = Path.Combine("Emails", "templates", "quotation-pt.html");
        string template = File.ReadAllText(templatePath);

        // Changes....
        template = FillProductsContent(template, orderToken, executeUser);
        template = FillClientInfo(template, client);
        template = FillAddressInfo(template, orderItem, client);
        template = FillPrimaveraDetails(template, primaveraDoc);
        template = FillOrderDetails(template, orderItem, executeUser);
        template = template.Replace("{{masterferroLogo}}", ConfigManager.Logo);

        return template;
    }

    private static string FillProductsContent(string template, string orderToken, string executeUser)
    {
        List<OrderProductItem> orderProducts = OrderProductModel.GetOrderProducts(orderToken, executeUser);

        List<string> productIds = [.. orderProducts.Select(x => x.product_catalog_id.ToString())];
        List<ProductCatalogItem> catalogs = ProductCatalogModel.GetProductCatalogsByIds(productIds, false, executeUser);
        Dictionary<int, ProductCatalogItem> catalogDict = catalogs.ToDictionary(x => x.id);

        List<ProductUnitItem> units = ProductUnitModel.GetProductUnits(executeUser);

        List<ProductConversionItem> conversions = ProductConversionModel.GetProductConversions(executeUser);
        Dictionary<string, List<ProductConversionItem>> conversionDict = conversions.GroupBy(x => x.product_code).ToDictionary(x => x.Key, x => x.ToList());

        StringBuilder tableContent = new();
        foreach (OrderProductItem orderProduct in orderProducts)
        {
            // Use helper function to convert quantities
            (decimal quantityInUnits, decimal quantityInBaseUnit) =
                OrderProductModel.GetConvertedQuantities(
                        orderProduct, catalogDict, conversionDict, units);

            ProductCatalogItem catalog = catalogDict[orderProduct.product_catalog_id];

            // Format quantities as "1 000 000,00"
            string quantityInUnitsFormatted = quantityInUnits.ToString("N2");
            string quantityInBaseUnitFormatted = quantityInBaseUnit.ToString("N2");

            // Calculate pricing details
            decimal IVA = 1.23m;
            decimal unitaryPrice = Math.Round(orderProduct.calculated_price, 3, MidpointRounding.AwayFromZero);
            decimal productPriceRounded = Math.Round(unitaryPrice * quantityInBaseUnit, 2, MidpointRounding.AwayFromZero);
            decimal productPrice = Math.Round(productPriceRounded * IVA, 2, MidpointRounding.AwayFromZero);
            decimal productTotal = Math.Round(productPrice, 2, MidpointRounding.AwayFromZero);
            decimal discount = Math.Round(0.00m, 2, MidpointRounding.AwayFromZero);

            int IvaForTable = 23;

            tableContent.AppendLine("<tr>");
            // Artigo
            tableContent.AppendLine($"<td>{catalog.product_code}</td>");
            // Descriçao
            tableContent.AppendLine($"<td>{catalog.description_full}</td>");
            // Qtd.
            tableContent.AppendLine($"<td>{quantityInBaseUnitFormatted}</td>");
            // Un.
            tableContent.AppendLine($"<td>{catalog.unit}</td>");
            // QTD UN
            tableContent.AppendLine($"<td>{quantityInUnitsFormatted}</td>");
            // Pr. Unitário
            tableContent.AppendLine($"<td>{unitaryPrice}</td>");
            // Desc.
            tableContent.AppendLine($"<td>{discount}</td>");
            // IVA
            tableContent.AppendLine($"<td>{IvaForTable}</td>");
            // Valor
            tableContent.AppendLine($"<td>{productTotal}</td>");
            tableContent.AppendLine("</tr>");
        }

        string productsTable = tableContent.ToString();
        template = template.Replace("{{productsTable}}", productsTable);

        return template;
    }

    private static string FillClientInfo(string template, MFPrimaveraClientItem client)
    {
        StringBuilder content = new();
        /*
         The content should be as follows:
        <p>SERR. JOSE ROCHA &amp; SOARES, LDA.</p>
        <p>PARQUE INDUSTRIAL DE RUÃES, PAV.23</p>
        <p>MIRE DE TIBÃES</p>
        <p>4700-565 BRAGA</p>
        */
        content.AppendLine($"<p>{client.Nome}</p>");
        content.AppendLine($"<p>{client.Morada}</p>");
        content.AppendLine($"<p>{client.Localidade}</p>");
        content.AppendLine($"<p>{client.CodPostal} {client.CodPostalLocalidade}</p>");

        string clientInfo = content.ToString();

        template = template.Replace("{{clientInfo}}", clientInfo);
        template = template.Replace("{{clientContribuinte}}", client.Contribuinte);
        template = template.Replace("{{clientCode}}", client.Cliente);

        return template;
    }

    private static string FillAddressInfo(string template, OrderItem order, MFPrimaveraClientItem client)
    {
        /*
            Content should be as follows:
            <p>PARQUE INDUSTRIAL DE RUÃES, PAV.23</p>
            <p>MIRE DE TIBÃES</p>
            <p>4700-565 BRAGA</p>
            <p>Portugal</p>
        */

        string deliveryAddress = GetAddressContent(order, client);

        template = template.Replace("{{deliveryAddress}}", deliveryAddress);

        return template;
    }

    private static string GetAddressContent(OrderItem order, MFPrimaveraClientItem client)
    {
        StringBuilder content = new();

        if (!order.is_delivery)
        {
            // if not delivery, the default address should be the client address
            content.AppendLine($"<p>{client.Morada}</p>");
            content.AppendLine($"<p>{client.Localidade}</p>");
            content.AppendLine($"<p>{client.CodPostal} {client.CodPostalLocalidade}</p>");
            content.AppendLine($"<p>Portugal</p>");
            return content.ToString();
        }

        // The city format is "Municipality, District, Locality"
        // If the format is not as expected, we should throw an error

        if (string.IsNullOrEmpty(order.city) || string.IsNullOrEmpty(order.locality) || string.IsNullOrEmpty(order.district))
        {
            throw new ArgumentException("City, locality or district are missing");
        }

        string locality = order.locality;
        string municipality = order.city;
        string district = order.district;

        content.AppendLine($"<p>{order.address}</p>");
        content.AppendLine($"<p>{municipality}, {district}</p>");
        content.AppendLine($"<p>{order.postal_code} {locality}</p>");
        content.AppendLine($"<p>Portugal</p>");

        return content.ToString();
    }

    private static string FillOrderDetails(string template, OrderItem order, string executeUser)
    {
        OrderTotalItem total = OrderModel.CalculateOrderTotal(order.token, executeUser);

        // The incident is the liquid amount with the discount, at which the IVA will be applied
        string incidenteQtd = total.totalDiscount.ToString("N2");
        string orderTotal = total.totalDiscountPlusTax.ToString("N2");

        // the Valor Mercadoria is the total amount, without the discount. the discount will be applied later
        string orderValueFf = total.total.ToString("N2");

        decimal incidenteTotalTax = total.totalDiscountPlusTax - total.totalDiscount;
        string incidenteTotalTaxFf = incidenteTotalTax.ToString("N2");

        decimal totalDiscount = total.totalDiscount - total.total;
        string totalDiscountFf = totalDiscount.ToString("N2");

        decimal financialDiscount = 0;
        string financialDiscountFf = financialDiscount.ToString("N2");

        decimal totalDiscountTotal = totalDiscount + financialDiscount;
        string totalDiscountTotalFf = totalDiscountTotal.ToString("N2");

        template = template.Replace("{{incidenteQtd}}", incidenteQtd);
        template = template.Replace("{{incidenteTotal}}", incidenteTotalTaxFf);

        template = template.Replace("{{valorMercadoria}}", orderValueFf);
        template = template.Replace("{{orderTotal}}", orderTotal);
        template = template.Replace("{{descontosComerciais}}", totalDiscountFf);
        template = template.Replace("{{descontoFinanceiro}}", financialDiscountFf);


        template = template.Replace("{{descontoFinal}}", totalDiscountTotalFf);

        decimal clientRating = OrderRatingModel.GetOrderWeightedRating(order, executeUser);
        string clientRatingFf = clientRating.ToString("N2");
        template = template.Replace("{{descontoCliente}}", clientRatingFf);

        return template;
    }

    private static string FillPrimaveraDetails(string template, PrimaveraDocument primaveraDoc)
    {
        // Primavera Info are CondPagamento, which will be able to calculate dataVencimento
        // Also the numeroDocumento
        int daysToPay = Convert.ToInt16(primaveraDoc.CondPag);
        string condPagamento = $"Pagamento a {daysToPay} dias";

        template = template.Replace("{{documentNumber}}", primaveraDoc.Documento);
        template = template.Replace("{{condPagamento}}", condPagamento);

        string dataDoc = primaveraDoc.DataDoc.ToString("yyyy-MM-dd");
        string dataDocHora = primaveraDoc.DataDoc.ToString("yyyy-MM-dd / HH:mm");
        string dataVencimento = primaveraDoc.DataVenc.ToString("yyyy-MM-dd");

        template = template.Replace("{{dataDoc}}", dataDoc);
        template = template.Replace("{{dataDocHora}}", dataDocHora);
        template = template.Replace("{{vencimentoFatura}}", dataVencimento);

        template = template.Replace("{{referencia}}", primaveraDoc.Referencia);
        template = template.Replace("{{referencias}}", primaveraDoc.Referencia);

        template = template.Replace("{{vendedor}}", primaveraDoc.Linhas[0].Vendedor);

        return template;
    }

}
