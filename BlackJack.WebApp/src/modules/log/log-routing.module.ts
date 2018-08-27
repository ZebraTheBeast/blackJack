import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

import { LogComponent } from "../../components/log.component";


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