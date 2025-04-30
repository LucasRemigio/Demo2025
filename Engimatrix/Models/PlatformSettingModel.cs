// // Copyright (c) 2024 Engibots. All rights reserved.

using System.ComponentModel;
using System.Reflection;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.Utils;
using engimatrix.Views;
using Engimatrix.ModelObjs;

namespace Engimatrix.Models;


public enum PlatformSettingId

{
    [Description("quotation-expiration-time")]
    QuotationExpirationTime = 1
}

// Extension method to get the string value
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        DescriptionAttribute? attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}

public static class PlatformSettingModel
{

    public static PlatformSettingItem? GetById(int id, string executeUser)
    {
        Dictionary<string, string> parameters = new()
        {
            { "@SettingId", id.ToString() }
        };

        string query = @"SELECT id, setting_key, setting_value, created_at, updated_at, updated_by FROM platform_setting WHERE id = @SettingId";

        return SqlExecuter.ExecuteFunction<PlatformSettingItem>(query, parameters, executeUser, false, "GetPlatformSettingsById").FirstOrDefault();
    }

    public static List<PlatformSettingItem> GetAll(string executeUser)
    {
        string query = @"SELECT id, setting_key, setting_value, created_at, updated_at, updated_by FROM platform_setting ";

        return SqlExecuter.ExecuteFunction<PlatformSettingItem>(query, [], executeUser, false, "GetAllPlatformSettings");
    }

    public static PlatformSettingItem? Patch(int settingId, string value, string executeUser)
    {
        // check if the settingId is valid
        if (!Enum.IsDefined(typeof(PlatformSettingId), settingId))
        {
            throw new ArgumentException("Invalid setting key: " + settingId);
        }

        // try parse the string to int
        if (!int.TryParse(value, out int intValue))
        {
            throw new ArgumentException("Invalid value for setting key: " + settingId);
        }

        // check if the value is valid for the setting key
        if (settingId == (int)PlatformSettingId.QuotationExpirationTime && (intValue < 0 || intValue > 30))
        {
            throw new ArgumentException("Invalid value for setting key: " + settingId);
        }

        Dictionary<string, string> parameters = new()
        {
            { "@SettingId", settingId.ToString() },
            { "@SettingValue", intValue.ToString() },
            { "@UpdatedBy", executeUser }
        };

        string query = @"UPDATE platform_setting SET setting_value = @SettingValue, updated_by = @UpdatedBy WHERE id = @SettingId";

        SqlExecuter.ExecuteFunction(query, parameters, executeUser, false, "UpdatePlatformSetting");

        UpdatePlatformConfigById(settingId, value);

        return GetById(settingId, executeUser);
    }

    private static void UpdatePlatformConfigById(int settingId, string value)
    {
        // get the key description by the id
        string description = GetDescriptionById(settingId);

        UpdatePlatformConfigByKey(description, value);
    }

    private static void UpdatePlatformConfigByKey(string key, string value)
    {
        if (!ConfigManager.platformSettings.TryGetValue(key, out string? currentValue))
        {
            throw new ArgumentException($"Key '{key}' not found in platform settings.");
        }

        ConfigManager.platformSettings[key] = value;
        Log.Info($"UpdatePlatformConfigByKey - Key: {key} - Old Value: {currentValue} - New Value: {value}");
    }

    private static string GetDescriptionById(int id)
    {
        if (!Enum.IsDefined(typeof(PlatformSettingId), id))
        {
            throw new ArgumentException("Invalid setting key: " + id);
        }

        return ((PlatformSettingId)id).GetDescription();
    }

}
