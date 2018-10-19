import { NgModule } from '@angular/core';
import { GridModule } from '@progress/kendo-angular-grid';
import { SharedModule } from 'src/app/shared/shared.module';
import { LogRoutingModule } from 'src/app/log/log-routing.module';

import { LogComponent } from 'src/app/log/log/log.component';

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