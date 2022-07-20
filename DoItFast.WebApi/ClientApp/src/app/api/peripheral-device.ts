import { Response, FilterResponse } from "./response"
import { Paging, Order } from "./filter-request"

export interface PeripheralDevice {
    id: string;
    vendor: string;
    peripheralDeviceStatusId: number;
    created: Date;
}

export interface PeripheralDeviceStatus {
    name: string
    id: number
}

export interface PeripheralDeviceFilter {
    paging: Paging;
    order: Order;
}

export interface PeripheralDeviceWithGateway {
    id: string;
    vendor: string;
    peripheralDeviceStatusId: number;
    created: Date;
    readableName: string;
    serialNumber: string;
    ipAddress: string;
    status: string;
}

export interface PeripheralDeviceToDelete {
    id: string;
    serialNumber: string;
}

export interface PeripheralDeviceStatusResponse extends Response<PeripheralDeviceStatus[]> { }
export interface PeripheralDeviceWithGatewayResponse extends
    Response<FilterResponse<PeripheralDeviceWithGateway[]>> { }

