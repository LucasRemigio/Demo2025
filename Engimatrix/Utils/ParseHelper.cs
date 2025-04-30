// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;

namespace engimatrix.Utils
{
    public static class ParseHelper
    {
        public static decimal ParseDecimal(string value) => ParseDecimal(value, 0);
        public static decimal ParseDecimal(string value, decimal defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        public static int ParseInt(string value) => ParseInt(value, 0);
        public static int ParseInt(string value, int defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : int.Parse(value);
        }

        public static long ParseLong(string value) => ParseLong(value, 0);
        public static long ParseLong(string value, long defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : long.Parse(value);
        }

        public static bool ParseBool(string value) => ParseBool(value, false);
        public static bool ParseBool(string value, bool defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : bool.Parse(value);
        }

        public static double ParseDouble(string value) => ParseDouble(value, 0);
        public static double ParseDouble(string value, double defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : double.Parse(value);
        }

        public static DateOnly ParseDateOnly(string value) => ParseDateOnly(value, "dd-MM-yyyy", default);
        public static DateOnly ParseDateOnly(string value, DateOnly defaultValue) => ParseDateOnly(value, "dd-MM-yyyy", defaultValue);
        public static DateOnly ParseDateOnly(string value, string format) => ParseDateOnly(value, format, default);
        public static DateOnly ParseDateOnly(string value, string format, DateOnly defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : DateOnly.ParseExact(value, format);
        }

        public static DateTime ParseDateTime(string value) => ParseDateTime(value, "dd-MM-yyyy HH:mm:ss", default);
        public static DateTime ParseDateTime(string value, DateTime defaultValue) => ParseDateTime(value, "dd-MM-yyyy HH:mm:ss", defaultValue);
        public static DateTime ParseDateTime(string value, string format) => ParseDateTime(value, format, default);
        public static DateTime ParseDateTime(string value, string format, DateTime defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }

        public static string ParseString(string value) => string.IsNullOrEmpty(value) ? string.Empty : value;

    }
}
