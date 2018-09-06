﻿import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

import { LogComponent } from "src/modules/log/log.component";

const routes: Routes = [
    {
        path: '',
        component: LogComponent
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class LogRoutingModule { }