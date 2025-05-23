// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class SyncPrimaveraRatingsModel
{
    /*  ============================================================================================================================================================
    *               Historical Volume Ratings
    *   ============================================================================================================================================================
    */

    public static char GetHistoricalVolumeRating(decimal cumulativePercentage)
    {
        if (cumulativePercentage <= 0.8m)
        {
            return 'A';
        }
        else if (cumulativePercentage <= 0.95m)
        {
            return 'B';
        }
        else
        {
            return 'C';
        }
    }

    /*  =======================================================================================================================================================
    *               Credit Ratings
    *   =======================================================================================================================================================
    */

    public static async Task<SyncPrimaveraStats> SyncPrimaveraCreditRating(string executeUser)
    {
        // TODO: make sure the clients are already synced with primavera, or this will not find the clients
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int totalSyncs = 0;

        // get the primavera clients
        Dictionary<string, MFPrimaveraClientItem> primaveraClientsDic = await PrimaveraClientModel.GetPrimaveraClients();
        List<MFPrimaveraClientItem> primaveraClients = [.. primaveraClientsDic.Values];

        // get the clients ratings for credit
        List<ClientRatingItem> clientRatings = ClientRatingModel.GetClientRatingsByType((int)RatingTypes.RatingType.Credit, executeUser);

        // hash the ratings by client code
        Dictionary<string, ClientRatingItem> creditRatingsByClientCode = [];
        clientRatings.ForEach((clientRating) =>
        {
            creditRatingsByClientCode[clientRating.client_code] = clientRating;
        });

        List<RatingCriteriaItem> ratingCriterias = RatingCriteriaModel.GetRatingsByRatingType((int)RatingTypes.RatingType.Credit, executeUser);

        // for each client, get the credit rating and update the database
        foreach (MFPrimaveraClientItem primaveraClient in primaveraClients)
        {
            if (string.IsNullOrEmpty(primaveraClient.Cliente))
            {
                continue;
            }

            // get the rating
            char rating = GetCreditRating(ratingCriterias, primaveraClient.Avaliacao);

            // check if the rating is the already saved
            if (!creditRatingsByClientCode.TryGetValue(primaveraClient.Cliente, out ClientRatingItem? clientRating))
            {
                Log.Debug($"Client {primaveraClient.Cliente} not found in database. Creating new rating.");
                continue;
            }

            if (clientRating.rating == rating)
            {
                if (!string.IsNullOrEmpty(clientRating.updated_by))
                {
                    continue;
                }

                Log.Debug($"Keeping the same rating for client {primaveraClient.Cliente} to mark it as updated");
            }

            // patch the rating
            ClientRatingItem newClientRating = new ClientRatingItemBuilder()
                .SetClientCode(primaveraClient.Cliente)
                .SetRatingTypeId((int)RatingTypes.RatingType.Credit)
                .SetRating(rating)
                .Build();

            ClientRatingModel.PatchClientRating(newClientRating, executeUser);
            totalSyncs++;

            Log.Debug($"SyncPrimaveraCreditRating Client {primaveraClient.Cliente} updated from rating {clientRating.rating} to rating {rating}");
        }

        stopwatch.Stop();
        Log.Debug($"SyncPrimaveraCreditRating finished in {stopwatch.ElapsedMilliseconds}ms");
        SyncPrimaveraStats stats = new(stopwatch.ElapsedMilliseconds, totalSyncs);
        return stats;
    }

    /// <summary>
    /// This method will receive a value between 0 and 7, check the database parameters and return the correspondent rating
    /// </summary>
    public static char GetCreditRating(List<RatingCriteriaItem> criterias, string? evaluation)
    {
        char defaultRating = 'D';
        if (string.IsNullOrEmpty(evaluation))
        {
            return defaultRating;
        }

        // parametrize the credits ratings, database is A = 0-2, B = 3-5, C = 6-7, D = no rating
        if (!int.TryParse(evaluation, out int value))
        {
            return defaultRating;
        }

        // for each rating given, check its min and max. If the value is between them, return the rating
        foreach (RatingCriteriaItem criteria in criterias)
        {
            if (!TryParseRange(criteria.criteria, out int min, out int max))
            {
                continue;
            }

            if (value >= min && value <= max)
            {
                return criteria.rating;
            }
        }

        return defaultRating;
    }

    private static bool TryParseRange(string range, out int min, out int max)
    {
        min = max = 0;
        if (string.IsNullOrWhiteSpace(range))
        {
            return false;
        }

        string[] parts = range.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        string left = parts[0].Trim();
        string right = parts[1].Trim();

        return int.TryParse(left, out min) && int.TryParse(right, out max);
    }

    /*  =============================================================================================================================================================
    *               Payment Compliance Ratings
    *   =============================================================================================================================================================
    */

    public static async Task<SyncPrimaveraStats> SyncPrimaveraPaymentCompliances(string executeUser)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int totalSyncs = 0;

        // the all the clients from ourdatabase
        List<ClientItem> clients = ClientModel.GetClients(executeUser);

        // get the clients ratings for credit
        List<ClientRatingItem> clientRatings = ClientRatingModel.GetClientRatingsByType((int)RatingTypes.RatingType.PaymentCompliance, executeUser);

        // hash the ratings by client code
        Dictionary<string, ClientRatingItem> creditRatingsByClientCode = [];
        clientRatings.ForEach((clientRating) =>
        {
            creditRatingsByClientCode[clientRating.client_code] = clientRating;
        });

        /* for each client =>
                check all associated documents
                calculate average payment time
                check which rating is associated with the average payment time
                update the rating
        */
        foreach (ClientItem client in clients)
        {
            // calculate the average payment time and get the rating
            AveragePaymentTimeItem? averagePaymentTime = new AveragePaymentTimeItemBuilder().SetAverageDeadlineTimeDays(1).SetAveragePaymentTimeDays(2).Build();

            char rating = GetPaymentComplianceRating(averagePaymentTime);

            // update the rating
            if (!creditRatingsByClientCode.TryGetValue(client.code, out ClientRatingItem? clientRating))
            {
                Log.Debug($"Client {client.code} not found in database. Creating new rating.");
                continue;
            }

            if (clientRating.rating == rating)
            {
                if (!string.IsNullOrEmpty(clientRating.updated_by))
                {
                    continue;
                }

                Log.Debug($"Keeping the same rating for client {client.code} to mark it as updated");
            }

            // patch the rating
            ClientRatingItem newClientRating = new ClientRatingItemBuilder()
                .SetClientCode(client.code)
                .SetRatingTypeId((int)RatingTypes.RatingType.PaymentCompliance)
                .SetRating(rating)
                .Build();

            ClientRatingModel.PatchClientRating(newClientRating, executeUser);

            string message = $"SyncPrimaveraPaymentCompliances Client {client.code} \n " +
                $"Average payment time: {averagePaymentTime?.average_payment_time_days} days \n" +
                $"Average deadline payment time: {averagePaymentTime?.average_deadline_time_days} days\n" +
                $"=> Updated from rating {clientRating.rating} to rating {rating}";

            Log.Debug(message);
            totalSyncs++;
        }

        stopwatch.Stop();
        SyncPrimaveraStats stats = new(stopwatch.ElapsedMilliseconds, totalSyncs);
        Log.Debug($"SyncPrimaveraPaymentCompliances finished in {stopwatch.ElapsedMilliseconds}ms");
        return stats;
    }

    public static AveragePaymentTimeItem? CalculateAveragePaymentTime(List<MFPrimaveraInvoiceItem>? invoices)
    {
        if (invoices == null)
        {
            return null;
        }

        if (invoices.Count <= 0)
        {
            return null;
        }

        // the invoice is paid if the ValorPendente is 0 (zero), even if the dataLiquidacao is null
        // there are two types of invoices:
        // The ones that are paid => ValorLiquidacao = ValorTotal and DataLiquidaçao filled in
        // and the ones that are unpaid => Valor liquidaçao < ValorTotal and DataLiquidaçao is null

        // Notes: There are invoices with ValorLiquidacao = ValorTotal (they are paid) but DataLiquidaçao is null

        float totalPaymentTime = 0;
        float totalDaysRelativeToDeadline = 0;

        int totalInvoices = 0;
        int totalDeadlineInvoices = 0;

        foreach (MFPrimaveraInvoiceItem invoice in invoices)
        {
            if (string.IsNullOrEmpty(invoice.DataLiquidacao.ToString()) ||
                        invoice.ValorLiquidacao != invoice.ValorTotal)
            {
                continue;
            }

            DateTime paymentDate = invoice.DataLiquidacao!.Value;
            totalPaymentTime += (float)(paymentDate - invoice.DataDoc).TotalDays;
            totalInvoices++;

            if (string.IsNullOrEmpty(invoice.DataVencimento.ToString()))
            {
                continue;
            }

            DateTime deadlineDate = invoice.DataVencimento!.Value;
            totalDaysRelativeToDeadline += (float)(paymentDate - deadlineDate).TotalDays;
            totalDeadlineInvoices++;
        }

        float? averagePaymentTime = totalInvoices > 0 ? totalPaymentTime / totalInvoices : null;
        float? averageDeadlineTime = totalDeadlineInvoices > 0 ? totalDaysRelativeToDeadline / totalDeadlineInvoices : null;

        AveragePaymentTimeItem averagePaymentTimeItem = new AveragePaymentTimeItemBuilder()
            .SetAveragePaymentTimeDays(averagePaymentTime)
            .SetAverageDeadlineTimeDays(averageDeadlineTime)
            .Build();

        return averagePaymentTimeItem;
    }

    private static char GetPaymentComplianceRating(List<MFPrimaveraInvoiceItem> invoices)
    {
        if (invoices.Count <= 0 || invoices == null)
        {
            throw new ArgumentNullException("Invoices list is null or empty");
        }

        AveragePaymentTimeItem? averagePaymentTime = CalculateAveragePaymentTime(invoices);
        if (averagePaymentTime == null)
        {
            return 'D';
        }

        return GetPaymentComplianceRating(averagePaymentTime);
    }

    private static char GetPaymentComplianceRating(AveragePaymentTimeItem? averagePaymentTime)
    {
        if (averagePaymentTime == null)
        {
            return 'D';
        }

        /* Ratings:
        *  <= 30 days and pays => A
        *  <= 60 days and pays => B
        *   > 60 days and pays => C
        *  Does not pay, > 15 days after deadline => D
        */

        // Like this, we can first check if the client has above 15 days of average deadline time, indicating
        // he, in average, tends to pay after the deadline date. After that checking, e check how much time
        // the client usually takes to pay, if it is under 30 days, under 60 days, or after 60 days

        // TODO: Retrieve the range values from the database dinamically

        if (averagePaymentTime.average_deadline_time_days > 15)
        {
            return 'D';
        }

        return averagePaymentTime.average_payment_time_days switch
        {
            <= 30 => 'A',
            <= 60 => 'B',
            > 60 => 'C',
            _ => 'D'
        };
    }
}
