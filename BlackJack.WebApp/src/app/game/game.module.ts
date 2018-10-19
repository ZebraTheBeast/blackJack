import { NgModule } from '@angular/core';

import { GameRoutingModule } from 'src/app/game/game-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

import { GameComponent } from 'src/app/game/game/game.component';


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