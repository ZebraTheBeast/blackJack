import { NgModule } from '@angular/core';
import { GridModule } from '@progress/kendo-angular-grid';
import { SharedModule } from 'src/modules/shared.module';
import { LogRoutingModule } from 'src/modules/log/log-routing.module';

import { LogComponent } from 'src/modules/log/log.component';

@NgModule({
    imports: [
        SharedModule,
        LogRoutingModule,
        GridModule
    ],
    declarations: [
        LogComponent
    ]
})

export class LogModule { }