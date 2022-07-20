import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { GatewayFilterResponse, Gateway, GatewayResponse } from '../api/gateway';
import { GatewayFilter } from '../api/gateway';
import { BuildFilter } from '../helpers/build-filter';

@Injectable()
export class GatewayService {

    url: string;

    constructor(private http: HttpClient) {
        this.url = "api/gateway";
    }

    getPage(filter: GatewayFilter): Observable<GatewayFilterResponse> {
        let params = BuildFilter.Build(filter);
        return this.http.get<GatewayFilterResponse>(`${this.url}/page`, { params });
    }

    create(gateway: Gateway): Observable<GatewayResponse> {
        return this.http.post<GatewayResponse>(`${this.url}`, gateway);
    }

    update(gateway: Gateway): Observable<GatewayResponse> {
        return this.http.put<GatewayResponse>(`${this.url}`, gateway);
    }

    delete(id: string): Observable<GatewayResponse> {
        let params = new HttpParams().set("id", id);
        return this.http.delete<GatewayResponse>(`${this.url}`, { params: params });
    }


}