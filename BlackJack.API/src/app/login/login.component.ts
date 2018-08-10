import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';
import { ErrorComponent } from '../error/error.component';

@Component({
    selector: 'app-login',
    styleUrls: ['../../../Content/bootstrap.css', '../../../Content/Site.css'],
    templateUrl: './login.component.html'
})

export class LoginComponent {
    name: string;
    humanId: number;

    @ViewChild(ErrorComponent) errorComponent: ErrorComponent;

    constructor(private loginService: LoginService, private router: Router) { }

    startGame(): void {
        this.loginService.startGame(this.name).subscribe(
            humanId => {
                this.router.navigate([`game/${humanId}`]);
            },
            response => {
                this.errorComponent.showError(response);
            }
        )
    }

    loadGame(): void {
        this.loginService.loadGame(this.name).subscribe(
            humanId => {
                this.router.navigate([`game/${humanId}`]);
            },
            response => {
                this.errorComponent.showError(response);
            }
        )
    }
}
