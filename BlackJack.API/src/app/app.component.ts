import { Game } from '../game/game';
import { Player } from '../game/Player';
import { Card } from '../game/card';
import { Component, OnInit } from '@angular/core';
import { GameService } from '../game.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  game: Game;

  constructor(private gameSerivce: GameService) { }

  ngOnInit() {
    this.getGame();
  }

  getGame(): void {
    this.gameSerivce.getGame().subscribe(game => this.game = game);
  }
}
