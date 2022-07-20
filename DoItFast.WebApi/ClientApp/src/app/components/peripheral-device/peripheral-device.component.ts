import { Component, OnInit, ViewChild } from '@angular/core';
import { MessageService, ConfirmationService, LazyLoadEvent } from 'primeng/api';
import { Router } from '@angular/router';
import { PeripheralDeviceService } from '../../service/peripheral-device.service';
import { PeripheralDeviceStatus, PeripheralDeviceWithGateway, PeripheralDeviceFilter } from '../../api/peripheral-device';
import { Paging, Order } from '../../api/filter-request';
import { SortOperation } from '../../api/enums/sort-operation';
import { TableColumn } from '../../api/table-column'
import { FillFilter } from '../../helpers/fill-filter';
import { ValidateBackendErrors } from '../../helpers/validate-backend-errors';
import { Table } from 'primeng/table';


@Component({
    templateUrl: './peripheral-device.component.html',
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
export class PeripheralDeviceComponent implements OnInit {
    totalRecords: number;
    loading: boolean;
    peripheralDeviceFilter: PeripheralDeviceFilter = {
        paging: <Paging>{
            page: 1,
            pageSize: 1
        },
        order: <Order>{
            sortOperation: SortOperation.ASC,
            sortBy: "Vendor"
        }
    }
    // peripheralDeviceFilter = [];
    peripheralDeviceStatusSelected: PeripheralDeviceStatus;
    peripheralDeviceStatus: PeripheralDeviceStatus[];
    peripheralDevices: PeripheralDeviceWithGateway[];
    peripheralDevice: PeripheralDeviceWithGateway;
    cols: TableColumn[] = [
        { field: "vendor", header: "Vendor", sortable: true },
        { field: "status", header: "Status", sortable: true },
        { field: "serialNumber", header: "Gateway Serial Number", sortable: true },
        { field: "readableName", header: "Gateway Readable Name", sortable: true },
        { field: "ipAddress", header: "Gateway Ip Address", sortable: true },
        { field: "created", header: "Created", sortable: true }
    ];
    @ViewChild('dt') table: Table;
    peripheralDeviceDialog = false;
    submitted = false;
    details = false;

    constructor(
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

    loadPeripheralDevice(event: LazyLoadEvent) {
        this.loading = true;
        this.peripheralDeviceFilter.order = FillFilter.FillOrder(event);
        this.peripheralDeviceFilter.paging = FillFilter.FillPaging(event);

        setTimeout(() => {
            this.peripheralDeviceService.getPage(this.peripheralDeviceFilter).subscribe({
                next: response => {
                    this.peripheralDevices = response.data.data;

                    for (let i = 0; i < this.peripheralDevices.length; i++) {
                        const status = this.peripheralDeviceStatus.find(p => p.id == this.peripheralDevices[i].peripheralDeviceStatusId)
                        this.peripheralDevices[i].status = status.name;
                    }

                    this.totalRecords = response.data.total;
                },
                error: e => {
                    var error = "Peripheral Devices could not be loaded"
                    this.messageService.add({ severity: 'error', summary: 'Rejected', detail: error, life: 5000 });
                    console.log(error);
                    console.log(e);
                }
            });
            this.loading = false;
        }, 1000);
    }

    deletePeripheralDevice(peripheralDevice: PeripheralDeviceWithGateway) {
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.peripheralDeviceService.delete(peripheralDevice).subscribe({
                    next: data => {
                        this.table.clear();
                        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Peripheral Device deleted', life: 5000 });
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
        this.peripheralDeviceDialog = false;
        this.submitted = false;
    }

    updatePeripheralDevice(peripheralDevice: PeripheralDeviceWithGateway) {
        this.peripheralDevice = { ...peripheralDevice };
        this.details = false;
        this.peripheralDeviceDialog = true;
        this.submitted = false;
    }

    savePeripheralDevice() {
        this.submitted = true;
        if (!this.peripheralDevice.vendor)
            return;
        this.peripheralDevice.peripheralDeviceStatusId = this.peripheralDeviceStatusSelected.id;
        this.peripheralDeviceService.update(this.peripheralDevice).subscribe({
            next: data => {
                this.table.clear();
                this.peripheralDeviceDialog = false;
                this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Peripheral Device updated', life: 5000 });
            },
            error: e => {
                ValidateBackendErrors.Validate(e, this.messageService, this.route);
            }
        });
    }

    showDetails(peripheralDevice: PeripheralDeviceWithGateway) {
        this.peripheralDevice = { ...peripheralDevice }
        this.details = true;
        this.peripheralDeviceDialog = true;
    }
}
