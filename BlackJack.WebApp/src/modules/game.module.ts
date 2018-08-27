import { NgModule } from '@angular/core';
import { GameComponent } from '../components/game.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GameRoutingModule } from './game-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        GameRoutingModule
    ],
    declarations: [
        GameComponent
    ]
})

export class GameModule { }