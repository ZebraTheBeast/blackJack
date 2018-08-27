import { Routes, RouterModule } from "@angular/router";
import { GameComponent } from "../components/game.component";
import { NgModule } from "@angular/core";
import { clampRange } from "@progress/kendo-angular-dateinputs/dist/es2015/util";


const routes: Routes = [
    {
        path: '',
        component: GameComponent
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class GameRoutingModule { }