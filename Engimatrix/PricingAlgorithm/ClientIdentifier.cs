// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Connector;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.If;
using System.Globalization;

namespace engimatrix.PricingAlgorithm;

public static class ClientIdentifier
{
    public static async Task<string> IdentifyClient(FilteredEmail filteredEmail)
    {
        // Getting the clients
        Dictionary<string, MFPrimaveraClientItem> primaveraDic = await PrimaveraClientModel.GetPrimaveraClients();
        List<MFPrimaveraClientItem> primaveraClients = [.. primaveraDic.Values];

        Log.Debug($"Reading email from {filteredEmail.email.from}");
        // First we try to match by email. It is instant and besides the resources we are saving with openAi, it
        // takes way less time, and is more accurate and less error prone
        List<MFPrimaveraClientItem> clientMatchedEmails = MatchClientByEmail(primaveraClients, filteredEmail.email.from);
        if (clientMatchedEmails.Count == 1)
        {
            Log.Debug($"Client identified instantly throught the email: {clientMatchedEmails[0].Email} => {clientMatchedEmails[0].Cliente}, {clientMatchedEmails[0].Nome}");
            return clientMatchedEmails[0].Cliente ?? string.Empty;
        }

        Cliente? cliente = await OpenAI.IdentifyClientFromEmail(filteredEmail);

        if (cliente == null)
        {
            Log.Error("Could not identify client from email");
            return string.Empty;
        }

        Log.Debug(cliente.ToString());

        List<MFPrimaveraClientItem> matchedClients = GetClosestMatchesForClientFromPrimavera(primaveraClients, cliente);
        // Return instantly if only one client is found
        if (matchedClients.Count == 1)
        {
            Log.Debug($"Client identified instantly: {matchedClients[0].Email} => {matchedClients[0].Cliente}, {matchedClients[0].Nome}");
            return matchedClients.FirstOrDefault()!.Cliente ?? string.Empty;
        }

        // Check if there are too many matches. In that case, the result is inconclusive
        if (matchedClients.Count > 100)
        {
            Log.Debug($"Too many clients found: {matchedClients.Count}");
            return string.Empty;
        }

        // if result is non conclusive, send to open ai to get the client
        Log.Debug($"Sending {matchedClients.Count} clients for OpenAI Matching");
        OpenAiPrimaveraClient? openAiClient = await OpenAI.MatchClientWithClosest(cliente, matchedClients);

        if (openAiClient == null)
        {
            Log.Debug("No client found in OpenAI");
            return string.Empty;
        }

        Log.Debug($"Client identified by OpenAI: {openAiClient.Cliente}, {openAiClient.Nome}");

        return openAiClient.Cliente ?? string.Empty;
    }

    public static List<MFPrimaveraClientItem> MatchClientByEmail(List<MFPrimaveraClientItem> primaveraClients, string email)
    {
        string clientEmail = EmailHelper.GetEmailBetweenTriangleBrackets(email);
        clientEmail = clientEmail.ToLower(CultureInfo.InvariantCulture).Trim();
        string emailDomain = clientEmail[(clientEmail.IndexOf('@') + 1)..];

        HashSet<string> commonDomains = new(StringComparer.OrdinalIgnoreCase)
            {
                "gmail.com", "hotmail.com", "outlook.com", "yahoo.com", "sapo.pt", "live.com", "icloud.com"
            };

        return primaveraClients
               .Where(c => !string.IsNullOrEmpty(c.Email))
               .Select(c => new { Client = c, Emails = new[] { c.EmailCliente, c.Email } })
               .Select(x => new
               {
                   x.Client,
                   NormalizedEmails = x.Emails
                       .Where(e => !string.IsNullOrEmpty(e))
                       .Select(e => e!.Trim().ToLower())
                       .ToList()
               })
               .Where(x => x.NormalizedEmails.Any(e =>
                   // Exact match
                   e.Equals(clientEmail, StringComparison.OrdinalIgnoreCase) ||
                   // Contains match - Sometimes emails are not well formatted
                   e.Contains(clientEmail, StringComparison.OrdinalIgnoreCase) ||
                   // Domain match - Sometimes its an employee sending the email, and not the general email of the company
                   (!commonDomains.Contains(emailDomain) &&
                    e.Contains(emailDomain, StringComparison.OrdinalIgnoreCase))
               ))
               .Select(x => x.Client)
               .Distinct()
               .ToList();
    }

    public static List<MFPrimaveraClientItem> GetClosestMatchesForClientFromPrimavera(List<MFPrimaveraClientItem> primaveraClients, Cliente client)
    {
        // First we filter by the most specific, certain, and less error prone field: the email
        string clientEmail = OpenAI.RemoveDiacritics(client.Email).Trim();
        List<MFPrimaveraClientItem> matchedClients = ClientHelper.MatchByParameter(primaveraClients, c => c.Email!, clientEmail);
        if (matchedClients.Count == 1) { return matchedClients; }

        matchedClients = ClientHelper.MatchByParameter(primaveraClients, c => c.EmailCliente!, client.Email);
        if (matchedClients.Count == 1) { return matchedClients; }

        // Then we filter by the enterprise specific details, like NIF (Unique to each enterprise), Postal Code (May include some enterprises, but not many)
        // The phone number (which can be of the employee, but if its the enterprise on, should be unique)
        // And then the enterprise name
        string cleanNif = OpenAI.RemoveDiacritics(client.Contribuinte).Trim();
        matchedClients = ClientHelper.MatchByParameter(primaveraClients, c => c.Contribuinte!, cleanNif);
        if (matchedClients.Count == 1) { return matchedClients; }

        string cleanPostalCode = OpenAI.RemoveDiacritics(client.CodPostal).Trim();
        matchedClients = ClientHelper.MatchByParameter(matchedClients, c => c.CodPostal!, cleanPostalCode);
        if (matchedClients.Count == 1) { return matchedClients; }

        string phoneNumber = ClientHelper.FormatPhoneNumber(client.Telemovel);
        matchedClients = ClientHelper.MatchByParameter(matchedClients, c => c.Telemovel!, phoneNumber);
        if (matchedClients.Count == 1) { return matchedClients; }

        List<MFPrimaveraClientItem> clientsByEnterprise = ClientHelper.MatchByWordList(matchedClients, c => c.Nome!, client.NomeEmpresa);
        if (clientsByEnterprise.Count == 1) { return clientsByEnterprise; }

        // If its a solo enterpreneur, we should check for the client name and address as well
        List<MFPrimaveraClientItem> clientsByAddress = ClientHelper.MatchByWordList(matchedClients, c => c.Morada!, client.Morada);
        List<MFPrimaveraClientItem> clientsByName = ClientHelper.MatchByWordList(matchedClients, c => c.Nome!, client.NomeCliente);

        List<MFPrimaveraClientItem> finalMatches = clientsByAddress
            .Concat(clientsByEnterprise)
            .Concat(clientsByName)
            .Distinct()
            .ToList();

        if (finalMatches.Count <= 0)
        {
            return matchedClients;
        }

        return finalMatches;
    }

}
