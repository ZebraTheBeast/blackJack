import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

const logUrl = '../../api/log/';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({ providedIn: 'root' })

export class LogService {

    constructor(private http: HttpClient) { }

    loadLogs(): Observable<any> {
        var loadLogUrl = logUrl + `GetLogs`;
        return this.http.get<any>(loadLogUrl);
    }
}
