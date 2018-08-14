import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from '../services/login.service';
import { MessageService } from '../services/message.service';

@Component({
    selector: 'app-login',
    styleUrls: ['../../Content/bootstrap.css', '../../Content/Site.css'],
    templateUrl: '../views/login.component.html'
})

export class LoginComponent {
    name: any;
    humanId: any;

    constructor(private loginService: LoginService, private router: Router, private messageService: MessageService) { }

    startGame(): void {
        this.loginService.startGame(this.name).subscribe(
            humanId => {
                this.router.navigate([`game/${humanId}`]);
            },
            response => {
                this.messageService.showError(response);
            }
        )
    }

    loadGame(): void {
        this.loginService.loadGame(this.name).subscribe(
            humanId => {
                this.router.navigate([`game/${humanId}`]);
            },
            response => {
                this.messageService.showError(response);
            }
        )
    }
}
