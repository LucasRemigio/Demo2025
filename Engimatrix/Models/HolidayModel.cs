// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.Config;

namespace engimatrix.Models;

// // Copyright (c) 2024 Engibots. All rights reserved.

public class HolidayItem
{
    public int day { get; set; }
    public int month { get; set; }

    public string description { get; set; }

    public HolidayItem(int day, int month, string description)
    {
        this.day = day;
        this.month = month;
        this.description = description;
    }

    public HolidayItem ToItem()
    {
        return new HolidayItem(this.day, this.month, this.description);
    }
}

public static class HolidaysModel

{
    public static List<DateTime> GetHolidays(int year)
    {
        List<DateTime> holidays = new List<DateTime>();

        foreach (HolidayItem item in GetAllHolidays(ConfigManager.defaultLanguage, "system"))
        {
            holidays.Add(new DateTime(year, item.month, item.day));
        }

        // Dynamic Holidays based om Easter date
        DateTime easter = CalculateEasterDate(year);

        holidays.Add(easter);                        // Domingo de Páscoa
        holidays.Add(easter.AddDays(-2));            // Sexta-feira Santa
        //holidays.Add(easter.AddDays(60));            // Corpo de Deus

        return holidays;
    }

    public static bool add(HolidayItem input, string user_operation)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("@day", input.day.ToString());
        param.Add("@month", input.month.ToString());
        param.Add("@description", input.description);

        SqlExecuter.ExecFunction("INSERT INTO holiday (day,month,description)   VALUES (@day,@month, @description)", param, user_operation, true, "Add Holiday");

        return true;
    }

    public static bool remove(HolidayItem input, string user_operation)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("@day", input.day.ToString());
        param.Add("@month", input.month.ToString());

        SqlExecuter.ExecFunction("DELETE FROM holiday  WHERE day= @day AND month= @month;", param, user_operation, true, "Remove Holiday");

        return true;
    }

    public static List<HolidayItem> GetAllHolidays(string language, string user_operation)
    {
        List<HolidayItem> result = new List<HolidayItem>();
        return result;

        HolidayItem rec = null;

        Dictionary<string, string> param = new Dictionary<string, string>();
        SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT day, month, description FROM holiday ORDER BY month ASC;", param, user_operation, true, "Get All Holidays");

        foreach (Dictionary<string, string> item in responsive.out_data)
        {
            int day = Convert.ToInt32(item["0"]);
            int month = Convert.ToInt32(item["1"]);
            string description = item["2"];

            rec = new HolidayItem(day, month, description);
            result.Add(rec.ToItem());
        }

        return result;
    }

    private static DateTime CalculateEasterDate(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int mes = (h + l - 7 * m + 114) / 31;
        int dia = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateTime(year, mes, dia);
    }
}
