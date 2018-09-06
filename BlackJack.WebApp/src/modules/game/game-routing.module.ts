import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

import { GameComponent } from "src/modules/game/game.component";

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