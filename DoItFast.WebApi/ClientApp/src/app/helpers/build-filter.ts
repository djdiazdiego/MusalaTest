import { HttpParams } from '@angular/common/http';
import { Filter } from '../api/filter-request';

export class BuildFilter {
    static Build(filter: Filter): HttpParams {

        let params = new HttpParams()
            .set("order.sortBy", filter.order.sortBy)
            .set("order.sortOperation", filter.order.sortOperation)
            .set("paging.page", filter.paging.page)
            .set("paging.pageSize", filter.paging.pageSize);

        return params;
    }
}