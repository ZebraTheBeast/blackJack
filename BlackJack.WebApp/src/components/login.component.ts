import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from '../services/login.service';
import { MessageService } from '../services/message.service';

@Component({
    selector: 'app-login',
    styleUrls: ['../../Content/bootstrap.css', '../../Content/Site.css'],
    templateUrl: '../views/login.component.html'
})

export class LoginComponent implements OnInit {
    name: any;
    botsAmount: any;
    humanId: any;

    ngOnInit() {
        this.botsAmount = 3;
    }

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

    botsAmountCheck(event: Event): void {
       
    }
}
