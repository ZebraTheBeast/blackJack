import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';

import { GameService } from 'src/game/game.service';
import { MessageService } from 'src/message/message.service';

@Component({
    selector: 'app-game',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css'],
    templateUrl: '/src/game/game.component.html'
})

export class GameComponent implements OnInit {
    betValue = 10;
    game: any;
    isDrawDisabled = false;
    isBetDisabled = false;

    constructor(private gameService: GameService, private router: Router, private messageService: MessageService) { }

    ngOnInit() {
        this.getGame();
    }

    getGame(): void {
        this.gameService.getGame().subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.messageService.showError(response);
                this.router.navigate([`login`]);
            });
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
            });
    }

    draw(): void {
        this.gameService.draw().subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.messageService.showError(response);
            });
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
            });
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
        if (this.game.human.cardListValue >= 21) {
            this.disableDraw();
        }

        if (this.game.human.betValue == 0) {
            this.disableDraw();
        }

        if (this.game.dealer.cardListValue == 21) {
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