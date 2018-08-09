import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class LoginService {

    private gameUrl = '../../api/values/';

    constructor(private http: HttpClient) { }

    startGame(playerName: string): Observable<number> {
        var startGameUrl = this.gameUrl + "StartGame";
        return this.http.post<number>(startGameUrl, JSON.stringify(playerName), httpOptions);
    }

    loadGame(playerName: string): Observable<number> {
        var startGameUrl = this.gameUrl + "LoadGame";
        return this.http.post<number>(startGameUrl, JSON.stringify(playerName), httpOptions);
    }

}
