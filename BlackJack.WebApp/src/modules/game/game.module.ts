import { NgModule } from '@angular/core';
import { SharedModule } from '../shared.module';

import { GameRoutingModule } from './game-routing.module';

import { GameComponent } from '../../components/game.component';



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