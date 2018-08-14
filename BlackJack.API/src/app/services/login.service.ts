import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

const gameUrl = '../../api/values/';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class LoginService {

    constructor(private http: HttpClient) { }

    startGame(playerName: string): Observable<number> {
        var startGameUrl = gameUrl + "StartGame";
        return this.http.post<number>(startGameUrl, JSON.stringify(playerName), httpOptions);
    }

    loadGame(playerName: string): Observable<number> {
        var loadGameUrl = gameUrl + `LoadGame?playerName=${playerName}`;
        return this.http.get<number>(loadGameUrl);
    }
}
