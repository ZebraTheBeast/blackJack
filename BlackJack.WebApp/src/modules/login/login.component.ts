import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from 'src/services/login.service';
import { MessageService } from 'src/services/message.service';

@Component({
    selector: 'app-login',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css'],
    templateUrl: './login.component.html'
})

export class LoginComponent {
    name: any;
    humanId: any;
    botsAmount = 3;

    constructor(private loginService: LoginService, private router: Router, private messageService: MessageService) { }

    startGame(): void {
        this.loginService.startGame(this.name, this.botsAmount).subscribe(
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
