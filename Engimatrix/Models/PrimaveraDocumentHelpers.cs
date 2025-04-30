// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using Microsoft.AspNetCore.Mvc;
using static engimatrix.Config.PrimaveraEnums;

namespace engimatrix.Models;

public static class PrimaveraDocumentHelpers
{
    public static string FixClientPostalCode(string? postalCode)
    {
        if (string.IsNullOrEmpty(postalCode))
        {
            return "0000-000";
        }

        // Ex: 1234-123
        if (postalCode.Contains("-") && postalCode.Length == 8)
        {
            return postalCode;
        }

        if (postalCode.Length == 7)
        {
            return postalCode.Insert(4, "-");
        }

        // only first 4 digits filled in
        if (postalCode.Length == 4)
        {
            return postalCode + "-000";
        }

        return postalCode;
    }

    public static string FixClientContribuinte(string? contribuinte)
    {
        string defaultContribuinte = "999999990";

        if (string.IsNullOrEmpty(contribuinte))
        {
            return defaultContribuinte;
        }

        if (contribuinte.Length != 9)
        {
            return defaultContribuinte;
        }

        return contribuinte;
    }


}
