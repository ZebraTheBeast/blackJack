import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { MessageService } from 'src/app/shared/services/message.service';
import { GameService } from 'src/app/shared/services/game.service';

@Component({
    selector: 'app-login',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css'],
    templateUrl: '/src/app/login/login/login.component.html'
})

export class LoginComponent {
    playerName: string;
    humanId: number;
    botsAmount = 3;

    constructor(private router: Router, private gameService: GameService) { }

    startGame(): void {
        this.gameService.setStartGameFlag(this.playerName, this.botsAmount);
        this.router.navigate([`game`]);
    }

    loadGame(): void {
        this.gameService.setLoadGameFlag(this.playerName);
        this.router.navigate([`game`]);
    }
}
