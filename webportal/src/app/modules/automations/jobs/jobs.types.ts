export interface JobsEntry {
    file64: string;
    state: string;
}

export interface FileInput {
    file: string;
}

export class Jobs {
    public id: string;
    public script_id: string;
    public script_name: string;
    public user_operation: string;
    public date_time: string;
    public state_operation: string;
    public job_details: String;
}
