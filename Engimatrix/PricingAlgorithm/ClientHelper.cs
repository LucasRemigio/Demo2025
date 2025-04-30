// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using System.Text.RegularExpressions;
using System.Text;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Config;
using System.Globalization;
using System.Linq.Expressions;
using engimatrix.Connector;

namespace engimatrix.PricingAlgorithm;

public static class ClientHelper
{

    // Helper method for matching a client by a parameter
    public static List<MFPrimaveraClientItem> MatchByParameter(
        List<MFPrimaveraClientItem> primaveraClients,
        Func<MFPrimaveraClientItem, string> propertySelector,
        string value
    )
    {
        if (string.IsNullOrEmpty(value)) { return primaveraClients; }

        List<MFPrimaveraClientItem> matchedClients = primaveraClients.FindAll(c =>
        {
            string propertyValue = propertySelector(c);
            return !string.IsNullOrEmpty(propertyValue) && propertyValue.Contains(value, StringComparison.OrdinalIgnoreCase);
        });

        if (matchedClients.Count >= 1) { return matchedClients; }

        return primaveraClients;
    }


    // Helper method to match a client by splitting the name into words and checking each word
    public static List<MFPrimaveraClientItem> MatchByWordList(
        List<MFPrimaveraClientItem> primaveraClients,
        Expression<Func<MFPrimaveraClientItem, string>> propertySelector,
        string value
    )
    {
        if (string.IsNullOrEmpty(value)) { return []; }

        string cleanValue = OpenAI.RemoveDiacritics(value).Trim();
        List<string> words = [.. cleanValue.Split(' ')];
        List<MFPrimaveraClientItem> filteredClients = primaveraClients;

        foreach (string word in words)
        {
            if (filteredClients.Count == 1) { break; }

            filteredClients = FilterPrimaveraClientListByParameter(filteredClients, propertySelector, word);
        }

        if (filteredClients.Count < primaveraClients.Count) { return filteredClients; }

        return [];
    }

    // Helper method for formatting phone numbers
    public static string FormatPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) { return string.Empty; }

        phoneNumber = OpenAI.RemoveDiacritics(phoneNumber).Trim();
        phoneNumber = phoneNumber.StartsWith("+351") ? phoneNumber[4..] : phoneNumber;
        phoneNumber = phoneNumber.Replace(" ", "");
        // Ensure it's 9 digits
        if (phoneNumber.Length > 9) { phoneNumber = phoneNumber[..9]; }
        return phoneNumber;
    }

    // Filter clients based on an expression (type-safe approach)
    private static List<MFPrimaveraClientItem> FilterPrimaveraClientListByParameter(
        List<MFPrimaveraClientItem> primaveraClients,
        Expression<Func<MFPrimaveraClientItem, string>> propertyExpression,
        string value
    )
    {
        if (string.IsNullOrEmpty(value) || propertyExpression == null) { return primaveraClients; }

        // Compile the expression to get the property value
        Func<MFPrimaveraClientItem, string> compiledExpression = propertyExpression.Compile();

        // Filter clients based on the value matching the property
        List<MFPrimaveraClientItem> filteredClients = primaveraClients.Where(
            c =>
                compiledExpression(c)?.Contains(value, StringComparison.OrdinalIgnoreCase) == true
        ).ToList();

        // If no clients were found, return the original list
        if (filteredClients.Count == 0) { return primaveraClients; }

        return filteredClients;
    }
}
