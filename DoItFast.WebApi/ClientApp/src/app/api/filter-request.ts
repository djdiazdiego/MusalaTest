import { SortOperation } from "./enums/sort-operation"

export interface Order {
    sortBy: string;
    sortOperation: SortOperation
}

export interface Paging {
    page: number;
    pageSize: number;
}

export interface Filter {
    paging: Paging;
    order: Order;
}