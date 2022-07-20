import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { PeripheralDeviceStatusResponse, PeripheralDeviceWithGatewayResponse, PeripheralDeviceToDelete, PeripheralDeviceFilter, PeripheralDeviceWithGateway } from '../api/peripheral-device';
import { BuildFilter } from '../helpers/build-filter';

@Injectable()
export class PeripheralDeviceService {

    url: string;

    constructor(private http: HttpClient) {
        this.url = "api/PeripheralDevice";
    }

    getAll(): Observable<PeripheralDeviceStatusResponse> {
        return this.http.get<PeripheralDeviceStatusResponse>(`${this.url}/all`);
    }

    getPage(filter: PeripheralDeviceFilter): Observable<PeripheralDeviceWithGatewayResponse> {
        let params = BuildFilter.Build(filter);
        return this.http.get<PeripheralDeviceWithGatewayResponse>(`${this.url}/page`, { params });
    }

    update(peripheralDevice: PeripheralDeviceWithGateway): Observable<PeripheralDeviceWithGatewayResponse> {
        return this.http.patch<PeripheralDeviceWithGatewayResponse>(`${this.url}/update-device`, peripheralDevice);
    }

    delete(peripheralDevice: PeripheralDeviceWithGateway): Observable<PeripheralDeviceWithGatewayResponse> {

        const peripheralDeviceToDelete = <PeripheralDeviceToDelete>{
            serialNumber: peripheralDevice.serialNumber,
            id: peripheralDevice.id
        }
        return this.http.delete<PeripheralDeviceWithGatewayResponse>(`${this.url}/delete-device`, { body: peripheralDeviceToDelete });
    }
}