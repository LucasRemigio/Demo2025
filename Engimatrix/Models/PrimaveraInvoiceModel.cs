// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class PrimaveraInvoiceModel
{
    public static async Task<List<MFPrimaveraInvoiceItem>> GetPrimaveraInvoices()
    {
        string oneYearAgo = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");

        PrimaveraListResponseItem<MFPrimaveraInvoiceItem> primaveraInvoices = await Primavera.GetListAsync<MFPrimaveraInvoiceItem>(
            ConfigManager.PrimaveraUrls.MFFaturas,
            999999,
            0,
            $"DataDoc >= \'\'{oneYearAgo}\'\'"
        );

        if (primaveraInvoices.IsError)
        {
            throw new PrimaveraApiErrorException(primaveraInvoices.Message!);
        }

        // Primavera, even though it might throw an error and not return anything, an
        // object on the list is always created, so we must check for 1
        if (primaveraInvoices.Data.Count <= 1)
        {
            throw new ResourceEmptyException("No invoices found in Primavera");
        }

        return primaveraInvoices.Data;
    }

    public static async Task<Dictionary<string, List<MFPrimaveraInvoiceItem>>> GetPrimaveraInvoicesHashedByClientCode()
    {
        List<MFPrimaveraInvoiceItem> primaveraInvoice = await GetPrimaveraInvoices();

        Dictionary<string, List<MFPrimaveraInvoiceItem>> hashedInvoices = [];
        foreach (MFPrimaveraInvoiceItem invoice in primaveraInvoice)
        {
            if (invoice.Entidade == null)
            {
                continue;
            }

            if (!hashedInvoices.TryGetValue(invoice.Entidade, out List<MFPrimaveraInvoiceItem>? value))
            {
                value = [];
                hashedInvoices[invoice.Entidade] = value;
            }

            value.Add(invoice);
        }

        return hashedInvoices;
    }


    public static async Task<List<MFPrimaveraInvoiceItem>> GetPrimaveraInvoicesByClientCodeLastYear(string client_code) => await GetPrimaveraInvoicesByClientCode(client_code, DateTime.Now.AddMonths(-12), null);
    public static async Task<List<MFPrimaveraInvoiceItem>> GetPendingPrimaveraInvoicesByClientCode(string client_code) => await GetPrimaveraInvoicesByClientCode(client_code, null, true);
    public static async Task<List<MFPrimaveraInvoiceItem>> GetPendingPrimaveraInvoicesByClientCodeLastYear(string client_code) => await GetPrimaveraInvoicesByClientCode(client_code, DateTime.Now.AddMonths(-12), true);
    public static async Task<List<MFPrimaveraInvoiceItem>> GetPrimaveraInvoicesByClientCode(string client_code, DateTime? startDate, bool? isPending)
    {
        string sqlWhere = $"Entidade = \'\'{client_code}\'\' ";

        if (startDate.HasValue)
        {
            string startDateStr = startDate.Value.ToString("yyyy-MM-dd");
            sqlWhere += $" AND DataDoc >= \'\'{startDateStr}\'\' ";
        }

        if (isPending.HasValue && isPending.Value == true)
        {
            sqlWhere += " AND ValorPendente > 0";
        }

        PrimaveraListResponseItem<MFPrimaveraInvoiceItem> primaveraInvoices = await Primavera.GetListAsync<MFPrimaveraInvoiceItem>(
            ConfigManager.PrimaveraUrls.MFFaturas,
            999999,
            0,
            sqlWhere
        );

        if (primaveraInvoices.IsError)
        {
            throw new PrimaveraApiErrorException(primaveraInvoices.Message!);
        }

        // No count validation, the client can have no invoices

        List<MFPrimaveraInvoiceItem> invoices = primaveraInvoices.Data.OrderBy(c => c.DataDoc).ToList();

        return invoices;
    }

    public static MFPrimaveraInvoiceTotalItem CalculateInvoicesTotal(List<MFPrimaveraInvoiceItem> invoices)
    {
        MFPrimaveraInvoiceTotalItem total = new();
        if (invoices.Count <= 0)
        {
            return new();
        }

        foreach (MFPrimaveraInvoiceItem invoice in invoices)
        {
            total.valor_pendente += invoice.ValorPendente;
            total.valor_total += invoice.ValorTotal;
            total.valor_liquidacao += invoice.ValorLiquidacao;
        }

        // round everything
        total.valor_pendente = Math.Round(total.valor_pendente, 2);
        total.valor_total = Math.Round(total.valor_total, 2);
        total.valor_liquidacao = Math.Round(total.valor_liquidacao, 2);

        return total;
    }
}
