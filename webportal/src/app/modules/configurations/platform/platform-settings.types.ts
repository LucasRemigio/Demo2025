/* eslint-disable @typescript-eslint/naming-convention */
import { BaseResponse } from '../clients/clients.types';

export interface PlatformSettingsResponse extends BaseResponse {
    platform_settings: PlatformSetting[];
}

export interface PlatformSettingResponse extends BaseResponse {
    platform_setting: PlatformSetting;
}

export interface PlatformSetting {
    id: number;
    setting_key: string;
    setting_value: string;
    created_at: string;
    updated_at: string;
    updated_by: string;
}

export interface UpdateAppSetting {
    id: number;
    setting_value: string;
}
