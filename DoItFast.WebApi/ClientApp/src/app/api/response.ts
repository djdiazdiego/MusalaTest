export interface Response<TData> {
    code: number;
    succeeded: boolean;
    message: string;
    errors: string[];
    data: TData
}

export interface ValidationResponse {
    Code: number;
    Succeeded: boolean;
    Message: string;
    Errors: { [key: string]: string }
}

export interface FilterResponse<TData> {
    total: number;
    data: TData;
}