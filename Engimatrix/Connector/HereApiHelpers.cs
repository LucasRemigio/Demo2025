// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;
using System.Text.Json;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Utils;

namespace engimatrix.Connector;

public static class HereApiHelper
{
    public static bool IsCoordinateValid(string coordinate)
    {
        // coordinate must be in {latitude},{longitude} format
        if (string.IsNullOrEmpty(coordinate))
        {
            return false;
        }

        string[] parts = coordinate.Split(',');
        if (parts.Length != 2)
        {
            return false;
        }

        if (!double.TryParse(parts[0], out double lat) || !double.TryParse(parts[1], out double lng))
        {
            return false;
        }

        return true;
    }
}