export interface Script {
    id: string;
    name: string;
    description: string;
    cron_job: string;
    status: string;
    url_script: string;
    main_file: string;
    script_update_last_time: string;
    version: string;
    last_execution: string;
    nextRun: string;
}

export interface Combobox {
    id: string;
    description: string;
    addressId?: string;
    price?: string;
    flag?: boolean;
    quantity?: string;
}