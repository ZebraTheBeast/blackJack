import { Game } from '../models/game/game';
import { Player } from '../models/game/Player';
import { Card } from '../models/game/card';
import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import { GameService } from '../services/game.service';
import { ActivatedRoute, Params } from '@angular/router';
import { ErrorComponent } from '../error/error.component';

@Component({
    selector: 'app-game',
    styleUrls: ['../../../Content/bootstrap.css', '../../../Content/Site.css'],
    templateUrl: './game.component.html'
})

export class GameComponent implements OnInit {
    betValue = 10;
    game: Game;

    isDrawDisabled = false;
    isBetDisabled = false;
    humanId: any;

    @ViewChild(ErrorComponent) errorComponent: ErrorComponent;

    @ViewChild('content') content: ElementRef;

    constructor(private gameService: GameService, private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.humanId = params['id'];
        });
        this.getGame();
    }

    getGame(): void {
        this.gameService.getGame(this.humanId).subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.errorComponent.showError(response);
            });
    }

    stand(): void {
        this.gameService.stand(this.game.Human.Id).subscribe(
            game => {
                this.game = game;
                this.disableDraw();
            },
            response => {
                this.errorComponent.showError(response);
            });
    }

    draw(): void {
        this.gameService.draw(this.game.Human.Id).subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.errorComponent.showError(response);
            });
    }

    bet(): void {
        this.gameService.bet(this.game.Human.Id, this.betValue).subscribe(
            game => {
                this.game = game;
                this.disableBet();
                this.checkGameStatus();
            },
            response => {
                this.errorComponent.showError(response);
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
        if (this.game.Human.Hand.CardListValue >= 21) {
            this.disableDraw();
        }

        if (this.game.Human.Hand.BetValue == 0) {
            this.disableDraw();
        }

        if (this.game.Dealer.Hand.CardListValue == 21) {
            this.disableDraw();
        }

        if ((this.game.Human.Hand.CardList.length != 0)
            && (this.game.Human.Hand.BetValue != 0)) {
            this.disableBet();
        }
    }
}
