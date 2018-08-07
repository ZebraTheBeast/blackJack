import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Game } from './game/game';

@Injectable({ providedIn: 'root' })

export class GameService {

  private gameUrl = '../../api/values/GetGameViewModel';

  constructor(private http: HttpClient) { }

  getGame(): Observable<Game> {
    return this.http.get<Game>(this.gameUrl);
  }
}
