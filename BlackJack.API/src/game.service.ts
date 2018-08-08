import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Game } from './game/game';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class GameService {

    private gameUrl = '../../api/values/';

    constructor(private http: HttpClient) { }

    getGame(): Observable<Game> {
        var getGameUrl = this.gameUrl + "GetGameViewModel";
        return this.http.get<Game>(getGameUrl);
    }

    stand(id: number): Observable<Game> {
        var standUrl = this.gameUrl + "Stand";
        return this.http.post<Game>(standUrl, id, httpOptions);
    }

    draw(id: number): Observable<Game> {
        var drawUrl = this.gameUrl + "Draw";
        return this.http.post<Game>(drawUrl, id, httpOptions);
    }

    bet(id: number, betValue: number): Observable<Game> {
        var betViewModel = {
            BetValue: betValue,
            Id: id
        };
        var drawUrl = this.gameUrl + "Bet";
        return this.http.post<Game>(drawUrl, betViewModel, httpOptions);
    }

    startGame(name: string): Observable<any> {
        var startGameUrl = this.gameUrl + "StartGame";
        return this.http.post(startGameUrl, name, httpOptions);
    }

    loadGame(name: string): Observable<any> {
        var loadGameUrl = this.gameUrl + "LoadGame";
        return this.http.post(loadGameUrl, name, httpOptions);
    }
}
