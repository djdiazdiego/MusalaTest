import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppMainComponent } from './app.main.component';
import { ErrorComponent } from './components/error/error.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { GatewayComponent } from './components/gateway/gateway.component';
import { PeripheralDeviceComponent } from './components/peripheral-device/peripheral-device.component';

const routes = [
    {
        path: '', component: AppMainComponent,
        children: [
            { path: '', component: GatewayComponent },
            { path: 'device', component: PeripheralDeviceComponent }
        ],
    },
    { path: 'pages/error', component: ErrorComponent },
    { path: 'pages/notfound', component: NotfoundComponent },
    { path: '**', redirectTo: 'pages/notfound' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes,
        {
            scrollPositionRestoration: 'enabled',
            anchorScrolling: 'enabled',
            useHash: false
        })],
    exports: [RouterModule]
})
export class AppRoutingModule {
}