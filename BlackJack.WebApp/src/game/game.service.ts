import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Game } from 'src/game/models/game';

import { environment } from 'src/environments/environment';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class GameService {

    gameId: number = 0;

    constructor(private http: HttpClient) { }

    getGame(): Observable<Game> {
        var getGameUrl = environment.gameUrl + `GetGame`;
        return this.http.post<Game>(getGameUrl, this.gameId, httpOptions);
    }

    stand(): Observable<Game> {
        var standUrl = environment.gameUrl + "Stand";
        return this.http.post<Game>(standUrl, this.gameId, httpOptions);
    }

    draw(): Observable<Game> {
        var drawUrl = environment.gameUrl + "Draw";
        return this.http.post<Game>(drawUrl, this.gameId, httpOptions);
    }

    bet(betValue: number): Observable<Game> {
        var responseBetGameViewModel = {
            betValue: betValue,
            gameId: this.gameId
        };
        var drawUrl = environment.gameUrl + "Bet";
        return this.http.post<Game>(drawUrl, responseBetGameViewModel, httpOptions);
    }

    setGameId(gameId: number): void {
        this.gameId = gameId;
    }
}
