import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GameComponent } from './game/game.component';
import { LoginComponent } from './login/login.component';
import { ErrorComponent } from './error/error.component';

@Component({
    selector: 'app-root',
    styleUrls: ['../../Content/bootstrap.css', '../../Content/Site.css'],
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
    showGame = true;
    showLogin = true;
    humanId: number;

    @ViewChild(ErrorComponent) errorComponent: ErrorComponent;

    toggleGame() { this.showGame = !this.showGame }
    toggleLogin() { this.showLogin = !this.showLogin }

    ngOnInit() {
        this.toggleGame();
    }

    onChanged(Id: any) {
        this.humanId = Id;
        this.toggleGame();
        this.toggleLogin();
    }

    onError(response: any) {
        this.errorComponent.showError(response);
    }
}
