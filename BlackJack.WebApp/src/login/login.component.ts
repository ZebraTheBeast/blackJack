import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from 'src/login/login.service';
import { MessageService } from 'src/message/message.service';
import { GameService } from 'src/game/game.service';

@Component({
    selector: 'app-login',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css'],
    templateUrl: '/src/login/login.component.html'
})

export class LoginComponent {
    name: any;
    humanId: any;
    botsAmount = 3;

    constructor(private loginService: LoginService, private router: Router, private messageService: MessageService, private gameService: GameService) { }

    startGame(): void {
        this.loginService.startGame(this.name, this.botsAmount).subscribe(
            response => {
                this.gameService.setGameId(response.gameId);
                this.router.navigate([`game`]);
            },
            errorResponse => {
                this.messageService.showError(errorResponse);
            }
        )
    }

    loadGame(): void {
        this.loginService.loadGame(this.name).subscribe(
            response => {
                this.gameService.setGameId(response.gameId);
                this.router.navigate([`game`]);
            },
            errorResponse => {
                this.messageService.showError(errorResponse);
            }
        )
    }
}
