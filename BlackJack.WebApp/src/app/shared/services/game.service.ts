import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { GetGameGameView } from 'src/app/shared/models/get-game-view.model';
import { StandGameView } from 'src/app/shared/models/stand-game-view.model';
import { DrawGameView } from 'src/app/shared/models/draw-game-view.model';
import { ResponseBetGameView } from 'src/app/shared/models/response-bet-game-view.model';

import { environment } from 'src/environments/environment';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class GameService {

    gameId: number = 0;
    isStartGame: boolean = false;
    isLoadGame: boolean = false;
    playerName: string;
    botsAmount: number;

    constructor(private http: HttpClient) { }

    startGame(): Observable<any> {
        var requestStartGameGameView = {
            playerName: this.playerName,
            botsAmount: this.botsAmount
        }
        var startGameUrl = `${environment.gameUrl}StartGame`;
        return this.http.post<any>(startGameUrl, requestStartGameGameView, httpOptions);
    }

    loadGame(): Observable<any> {
        var loadGameUrl = `${environment.gameUrl}LoadGame`;
        return this.http.post<any>(loadGameUrl, this.playerName, httpOptions);
    }

    stand(): Observable<StandGameView> {
        var standUrl = `${environment.gameUrl}Stand`;
        return this.http.post<StandGameView>(standUrl, this.gameId, httpOptions);
    }

    draw(): Observable<DrawGameView> {
        var drawUrl = `${environment.gameUrl}Draw`;
        return this.http.post<DrawGameView>(drawUrl, this.gameId, httpOptions);
    }

    bet(betValue: number): Observable<ResponseBetGameView> {
        var requestBetGameViewModel = {
            betValue: betValue,
            gameId: this.gameId
        };
        var drawUrl = `${environment.gameUrl}Bet`;
        return this.http.post<ResponseBetGameView>(drawUrl, requestBetGameViewModel, httpOptions);
    }

    setGameId(gameId: number): void {
        this.gameId = gameId;
    }

    setStartGameFlag(playerName: string, botsAmount: number): void {
        this.playerName = playerName;
        this.botsAmount = botsAmount;
        this.isStartGame = true;
        this.isLoadGame = false;
    }

    setLoadGameFlag(playerName: string): void {
        this.playerName = playerName;
        this.isStartGame = false;
        this.isLoadGame = true;
    }
}
