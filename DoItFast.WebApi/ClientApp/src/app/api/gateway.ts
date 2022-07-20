import { PeripheralDevice } from "./peripheral-device"
import { Paging, Order } from "./filter-request"
import { Response, FilterResponse } from "./response"

export interface Gateway {
    serialNumber: string;
    readableName: string;
    ipAddress: string;
    peripheralDevices: PeripheralDevice[];
}

export interface GatewayFilter {
    paging: Paging;
    order: Order;
}

export interface GatewayFilterResponse extends Response<FilterResponse<Gateway[]>> { }

export interface GatewayResponse extends Response<Gateway> { }
