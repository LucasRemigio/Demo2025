// // Copyright (c) 2024 Engibots. All rights reserved.


namespace engimatrix.ModelObjs;
public class PlatformSettingItem
{
    public int id { get; set; }
    public string setting_key { get; set; }
    public string setting_value { get; set; }
    // Currently the setting value can accept any type of value, from string to int to etc. So, to mitigate this in the future,
    // we can add 3 columns: DataType, Min, Max. 
    // Dataype will be number, string, float etc...
    // Min will be the minimum value for the setting if numeric, and the min length if string. The same for the Max
    // that will make both the frontend and backend validations easier, by building dynamic pages on the frontend
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public string? updated_by { get; set; }

    public PlatformSettingItem(int id, string setting_key, string setting_value, DateTime created_at, DateTime? updated_at, string? updated_by)
    {
        this.id = id;
        this.setting_key = setting_key;
        this.setting_value = setting_value;
        this.created_at = created_at;
        this.updated_at = updated_at;
        this.updated_by = updated_by;
    }

    public PlatformSettingItem()
    {

    }
}
