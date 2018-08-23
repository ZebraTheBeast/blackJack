import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

const gameUrl = '../../api/login/';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class LoginService {

    constructor(private http: HttpClient) { }

    startGame(playerName: string, botsAmount: number): Observable<number> {
        var loginViewModel = {
            playerName: playerName,
            botsAmount: botsAmount
        }

        var startGameUrl = gameUrl + "StartGame";
        return this.http.post<number>(startGameUrl, loginViewModel, httpOptions);
    }

    loadGame(playerName: string): Observable<number> {
        if (playerName == undefined) {
            playerName = "";
        }
        var loadGameUrl = gameUrl + `LoadGame?playerName=${playerName}`;
        return this.http.get<number>(loadGameUrl);
    }
}
