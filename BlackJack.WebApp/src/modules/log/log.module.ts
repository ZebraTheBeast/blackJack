import { NgModule } from '@angular/core';
import { SharedModule } from '../shared.module';

import { GridModule } from '@progress/kendo-angular-grid';

import { LogRoutingModule } from './log-routing.module';

import { LogComponent } from '../../components/log.component';

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