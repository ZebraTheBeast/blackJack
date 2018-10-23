import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { GameService } from 'src/app/shared/services/game.service';
import { MessageService } from 'src/app/shared/services/message.service';

@Component({
    selector: 'app-game',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css'],
    templateUrl: '/src/app/game/game/game.component.html'
})

export class GameComponent implements OnInit {
    betValue = 10;
    game: any;
    isDrawDisabled = false;
    isBetDisabled = false;

    constructor(private gameService: GameService, private router: Router, private messageService: MessageService) { }

    ngOnInit() {
        if (this.gameService.isStartGame) {
            this.startGame();
        }
        if (this.gameService.isLoadGame) {
            this.loadGame();
        }
    }

    startGame(): void {
        this.gameService.startGame().subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
                this.gameService.setGameId(game.gameId);
            },
            response => {
                this.messageService.showError(response);
                console.log(response);
                this.router.navigate([`login`]);
            }
        );
    }

    loadGame(): void {
        this.gameService.loadGame().subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
                this.gameService.setGameId(game.gameId);
            },
            response => {
                this.messageService.showError(response);
                this.router.navigate([`login`]);
            }
        );
    }

    stand(): void {
        this.gameService.stand().subscribe(
            game => {
                this.game = game;
                this.disableDraw();
                this.checkGameStatus();
            },
            response => {
                this.messageService.showError(response);
            }
        );
    }

    draw(): void {
        this.gameService.draw().subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.messageService.showError(response);
            }
        );
    }

    bet(): void {
        this.gameService.bet(this.betValue).subscribe(
            game => {
                this.game = game;
                this.disableBet();
                this.checkGameStatus();
            },
            response => {
                this.messageService.showError(response);
            }
        );
    }

    disableDraw(): void {
        this.isDrawDisabled = true;
        this.isBetDisabled = false;
        this.betValue = 10;
    }

    disableBet(): void {
        this.isDrawDisabled = false;
        this.isBetDisabled = true;
    }

    checkGameStatus(): void {
        if (this.game.human.cardsInHandValue >= 21) {
            this.disableDraw();
        }

        if (this.game.human.betValue == 0) {
            this.disableDraw();
        }

        if (this.game.dealer.cardInHandValue == 21) {
            this.disableDraw();
        }

        if ((this.game.human.hand.cardsInHand.length != 0)
            && (this.game.human.betValue != 0)) {
            this.disableBet();
        }

        if (this.game.human.points == 0) {
            this.game.options = "You lose, restart the game.";
            this.isDrawDisabled = true;
            this.isBetDisabled = true;
        }
    }
}