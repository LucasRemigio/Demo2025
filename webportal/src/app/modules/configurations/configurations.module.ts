import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, Router, RouterModule } from '@angular/router';
import { CancelReasonsComponent } from './cancel-reasons/cancel-reasons.component';
import { CancelReasonsModule } from './cancel-reasons/cancel-reasons.module';
import { ClientsComponent } from './clients/clients.component';
import { ClientsModule } from './clients/clients.module';
import { ProductsComponent } from './products/products.component';
import { ProductsModule } from './products/products.module';
import { PlatformModule } from './platform/platform.module';
import { PlatformComponent } from './platform/platform.component';

const routes: Route[] = [
    {
        path: 'cancel-reasons',
        component: CancelReasonsComponent,
    },
    {
        path: 'clients',
        component: ClientsComponent,
    },
    {
        path: 'products',
        component: ProductsComponent,
    },
    {
        path: 'platform',
        component: PlatformComponent,
    },
];

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        CancelReasonsModule,
        ClientsModule,
        ProductsModule,
        PlatformModule,
    ],
})
export class ConfigurationsModule {}
