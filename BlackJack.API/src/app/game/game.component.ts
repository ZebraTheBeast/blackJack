import { Game } from '../game/models/game';
import { Player } from '../game/models/Player';
import { Card } from '../game/models/card';
import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import { GameService } from '../game/game.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
    selector: 'app-game',
    styleUrls: ['../../../Content/bootstrap.css', '../../../Content/Site.css'],
    templateUrl: './game.component.html'
})
export class GameComponent implements OnInit {
    betValue = 10;
    game: Game;
    errorMessage: any;
    isDrawDisabled = false;
    isBetDisabled = false;

    @Input() humanId: number;
    @ViewChild('content') content: ElementRef;
    @Output() onError = new EventEmitter<any>();

    constructor(private gameService: GameService, private modalService: NgbModal) { }

    ngOnInit() {
        this.getGame();
    }

    getGame(): void {
        this.gameService.getGame(this.humanId).subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.onError.emit(response);
            });
    }

    stand(): void {
        this.gameService.stand(this.game.Human.Id).subscribe(
            game => {
                this.game = game;
                this.disableDraw();
            },
            response => {
                this.onError.emit(response);
            });
    }

    draw(): void {
        this.gameService.draw(this.game.Human.Id).subscribe(
            game => {
                this.game = game;
                this.checkGameStatus();
            },
            response => {
                this.onError.emit(response);
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
                this.onError.emit(response);
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
