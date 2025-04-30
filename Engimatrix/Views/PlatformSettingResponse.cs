// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class PlatformSettingResponse : GenericResponse
    {
        public PlatformSettingItem? platform_setting { get; set; }

        public PlatformSettingResponse(PlatformSettingItem platform_setting, int result_code, string language) : base(result_code, language)
        {
            this.platform_setting = platform_setting;
        }

        public PlatformSettingResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }

    public class PlatformSettingListResponse : GenericResponse
    {
        public List<PlatformSettingItem> platform_settings { get; set; }

        public PlatformSettingListResponse(List<PlatformSettingItem> platform_settings, int result_code, string language) : base(result_code, language)
        {
            this.platform_settings = platform_settings;
        }

        public PlatformSettingListResponse(int result_code, string language) : base(result_code, language)
        {
            this.platform_settings = [];
        }
    }

}