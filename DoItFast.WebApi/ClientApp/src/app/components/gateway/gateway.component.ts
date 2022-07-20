import { Component, OnInit, ViewChild } from '@angular/core';
import { MessageService, ConfirmationService, LazyLoadEvent } from 'primeng/api';
import { Router } from '@angular/router';
import { GatewayService } from '../../service/gateway.service';
import { PeripheralDeviceService } from '../../service/peripheral-device.service';
import { GatewayFilter, Gateway } from '../../api/gateway';
import { PeripheralDeviceStatus, PeripheralDevice } from '../../api/peripheral-device';
import { ValidationResponse } from '../../api/response';
import { Paging, Order } from '../../api/filter-request';
import { SortOperation } from '../../api/enums/sort-operation';
import { TableColumn } from '../../api/table-column'
import { FillFilter } from '../../helpers/fill-filter';
import { ValidateBackendErrors } from '../../helpers/validate-backend-errors';
import { Table } from 'primeng/table';


@Component({
    templateUrl: './gateway.component.html',
    providers: [MessageService, ConfirmationService],
    styleUrls: ['../../../assets/demo/badges.scss'],
    styles: [`
        :host ::ng-deep  .p-frozen-column {
            font-weight: bold;
        }

        :host ::ng-deep .p-datatable-frozen-tbody {
            font-weight: bold;
        }

        :host ::ng-deep .p-progressbar {
            height:.5rem;
        }
    `]
})
export class GatewayComponent implements OnInit {
    totalRecords: number;
    loading: boolean;
    gatewayFilter: GatewayFilter = <GatewayFilter>{
        paging: <Paging>{
            page: 1,
            pageSize: 1
        },
        order: <Order>{
            sortOperation: SortOperation.ASC,
            sortBy: "SerialNumber"
        }
    }
    // globalGatewayFilter = ["serialNumber", "readableName", "ipAddress"];
    gateways: Gateway[];
    gateway: Gateway;
    peripheralDeviceStatusSelected: PeripheralDeviceStatus[];
    peripheralDeviceStatus: PeripheralDeviceStatus[];
    cols: TableColumn[] = [
        { field: "serialNumber", header: "Serial Number", sortable: true },
        { field: "readableName", header: "Readable Name", sortable: true },
        { field: "ipAddress", header: "Ip Address", sortable: true }
    ];
    @ViewChild('dt') table: Table;
    gatewayDialog = false;
    submitted = false;
    edited = false;
    details = false;

    constructor(private gatewayService: GatewayService,
        private confirmationService: ConfirmationService,
        private messageService: MessageService,
        private route: Router,
        private peripheralDeviceService: PeripheralDeviceService) { }

    ngOnInit() {
        this.peripheralDeviceService.getAll().subscribe({
            next: response => {
                this.peripheralDeviceStatus = response.data;
            },
            error: e => {
                var error = "Peripheral Device status could not be loaded"
                this.messageService.add({ severity: 'error', summary: 'Rejected', detail: error, life: 5000 });
                console.log(error);
            }
        })
    }

    loadGateways(event: LazyLoadEvent) {
        this.loading = true;
        this.gatewayFilter.order = FillFilter.FillOrder(event);
        this.gatewayFilter.paging = FillFilter.FillPaging(event);

        setTimeout(() => {
            this.gatewayService.getPage(this.gatewayFilter).subscribe({
                next: response => {
                    this.gateways = response.data.data;
                    this.totalRecords = response.data.total;
                },
                error: e => {
                    var error = "Gateways could not be loaded"
                    this.messageService.add({ severity: 'error', summary: 'Rejected', detail: error, life: 5000 });
                    console.log(error);
                    console.log(e);
                }
            });
            this.loading = false;
        }, 1000);
    }

    deleteGateway(gateway: Gateway) {
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete ' + gateway.readableName + '?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.gatewayService.delete(gateway.serialNumber).subscribe({
                    next: data => {
                        this.table.clear();
                        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Gateway deleted', life: 5000 });
                    },
                    error: e => {
                        ValidateBackendErrors.Validate(e, this.messageService, this.route);
                        this.table.clear();
                    }
                });
            }
        });
    }

    hideDialog() {
        this.gatewayDialog = false;
        this.submitted = false;
    }

    addPeripheralDevice() {
        if (this.gateway.peripheralDevices.length < 10) {
            this.gateway.peripheralDevices.push(<PeripheralDevice>{
                vendor: "",
                peripheralDeviceStatusId: 0
            })
            this.peripheralDeviceStatusSelected.push(<PeripheralDeviceStatus>{})
        }
        else
            this.messageService.add({
                severity: 'error', summary: 'Rejected',
                detail: "It cannot be added, it has exceeded the maximum number of Peripheral Devices allowed: 10",
                life: 5000
            });
    }

    removePeripheralDevice(i: number) {
        this.gateway.peripheralDevices.splice(i, 1);
        this.peripheralDeviceStatusSelected.splice(i, 1);
    }

    createGateway() {
        this.edited = false;
        this.details = false;
        this.gateway = <Gateway>{};
        this.gateway.peripheralDevices = [];
        this.peripheralDeviceStatusSelected = []
        this.gatewayDialog = true;
        this.submitted = false;
    }

    updateGateway(gateway: Gateway) {
        this.fillGateway(gateway)
        this.edited = true;
        this.details = false;
        this.gatewayDialog = true;
        this.submitted = false;
    }

    saveGateway() {
        this.submitted = true;

        const invalid = this.gateway.peripheralDevices.some(v => v.vendor == undefined || v.vendor == "");

        if (!this.gateway.serialNumber || !this.gateway.readableName || !this.gateway.ipAddress || invalid)
            return;

        for (let i = 0; i < this.gateway.peripheralDevices.length; i++)
            this.gateway.peripheralDevices[i].peripheralDeviceStatusId = this.peripheralDeviceStatusSelected[i].id;

        if (!this.edited) {

            this.gatewayService.create(this.gateway).subscribe({
                next: data => {
                    this.table.clear();
                    this.gatewayDialog = false;
                    this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Gateway created', life: 5000 });
                },
                error: e => {
                    ValidateBackendErrors.Validate(e, this.messageService, this.route);
                }
            });
        }
        else {
            this.gatewayService.update(this.gateway).subscribe({
                next: data => {
                    this.table.clear();
                    this.gatewayDialog = false;
                    this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Gateway updated', life: 5000 });
                },
                error: e => {
                    ValidateBackendErrors.Validate(e, this.messageService, this.route);
                }
            });
        }
    }

    showDetails(gateway: Gateway) {
        this.fillGateway(gateway);
        this.details = true;
        this.gatewayDialog = true;
    }

    fillGateway(gateway: Gateway) {
        this.gateway = { ...gateway };
        this.peripheralDeviceStatusSelected = []
        if (this.gateway.peripheralDevices == undefined)
            this.gateway.peripheralDevices = [];

        for (let i = 0; i < this.gateway.peripheralDevices.length; i++) {
            const id = this.gateway.peripheralDevices[i].peripheralDeviceStatusId;
            var deviceStatus = this.peripheralDeviceStatus.find(p => p.id == id);
            this.peripheralDeviceStatusSelected.push({
                id: this.gateway.peripheralDevices[i].peripheralDeviceStatusId,
                name: deviceStatus.name
            });
        }
    }
}
