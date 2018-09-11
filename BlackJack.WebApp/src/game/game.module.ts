import { NgModule } from '@angular/core';
import { SharedModule } from 'src/shared.module';
import { GameRoutingModule } from 'src/game/game-routing.module';

import { GameComponent } from 'src/game/game.component';

@NgModule({
    imports: [
        SharedModule,
        GameRoutingModule
    ],
    declarations: [
        GameComponent
    ]
})

export class GameModule { }