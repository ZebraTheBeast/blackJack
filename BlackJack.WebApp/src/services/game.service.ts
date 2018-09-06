import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Game } from 'src/models/game';

const gameUrl = '../../api/game/';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class GameService {

    constructor(private http: HttpClient) { }

    getGame(id: number): Observable<Game> {
        var getGameUrl = gameUrl + `GetGameViewModel/${id}`;
        return this.http.get<Game>(getGameUrl);
    }

    stand(id: number): Observable<Game> {
        var standUrl = gameUrl + "Stand";
        return this.http.post<Game>(standUrl, id, httpOptions);
    }

    draw(id: number): Observable<Game> {
        var drawUrl = gameUrl + "Draw";
        return this.http.post<Game>(drawUrl, id, httpOptions);
    }

    bet(id: number, betValue: number): Observable<Game> {
        var betViewModel = {
            betValue: betValue,
            humanId: id
        };
        var drawUrl = gameUrl + "Bet";
        return this.http.post<Game>(drawUrl, betViewModel, httpOptions);
    }
}
