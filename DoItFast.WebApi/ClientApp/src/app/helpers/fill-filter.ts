import { LazyLoadEvent } from 'primeng/api';
import { Order, Paging } from '../api/filter-request';
import { SortOperation } from '../api/enums/sort-operation';

export class FillFilter {
    static FillPaging(event: LazyLoadEvent): Paging {

        const paging: Paging = {
            page: event.first != undefined ? event.first : 1,
            pageSize: event.rows != undefined ? event.rows : 10
        };

        return paging;
    }

    static FillOrder(event: LazyLoadEvent): Order {

        const order: Order = {
            sortOperation: event.sortOrder != undefined && event.sortOrder == -1 ? SortOperation.DESC : SortOperation.ASC,
            sortBy: event.sortField ?? "Id"
        };

        return order;
    }
}