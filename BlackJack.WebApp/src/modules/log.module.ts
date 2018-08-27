import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LogComponent } from '../components/log.component';
import { LogRoutingModule } from './log-routing.module';
import { GridModule } from '@progress/kendo-angular-grid';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        LogRoutingModule,
        GridModule
    ],
    declarations: [
        LogComponent
    ]
})

export class LogModule { }