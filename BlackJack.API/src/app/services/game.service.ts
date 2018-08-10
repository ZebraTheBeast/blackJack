import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Game } from '../models/game/game';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class GameService {

    private gameUrl = '../../api/values/';

    constructor(private http: HttpClient) { }

    getGame(id: number): Observable<Game> {
        var getGameUrl = this.gameUrl + `GetGameViewModel/${id}`;
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
            HumanId: id
        };
        var drawUrl = this.gameUrl + "Bet";
        return this.http.post<Game>(drawUrl, betViewModel, httpOptions);
    }
}
