import { NgModule } from '@angular/core';
import { SharedModule } from 'src/modules/shared.module';
import { GameRoutingModule } from 'src/modules/game/game-routing.module';

import { GameComponent } from 'src/modules/game/game.component';

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