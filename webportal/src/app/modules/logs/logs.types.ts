export interface LogsEntry
{
    file64: string;
    state: string;
}

export interface UpdateReceiptEntry
{
    email: string;
    state: string;
    
}

export interface FileInput
{
    file: string;
}

export interface Employee
{
    email: string;
    short_name:string;
    state: string;
    client: string;
    id: string;
    contractDate: string;
}

export class Product {
    public id: number;
    public operation: string;
    public user_operation: number;
    public dateTime: string;
    public operationContext: string;
    public stateOperation: string;
}
