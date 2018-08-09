import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { LoginService } from '../login/login.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';



@Component({
    selector: 'app-login',
    styleUrls: ['../../../Content/bootstrap.css', '../../../Content/Site.css'],
    templateUrl: './login.component.html'
})
export class LoginComponent {
    name: string;
    humanId: number;

    @Output() onChanged = new EventEmitter<number>();
    @Output() onError = new EventEmitter<any>();

    constructor(private loginService: LoginService, private modalService: NgbModal) { }


    startGame(): void {
        this.loginService.startGame(this.name).subscribe(
            humanId => {
                this.onChanged.emit(humanId);
            },
            response => {
                this.onError.emit(response);
            }
        )
    }

    loadGame(): void {
        this.loginService.loadGame(this.name).subscribe(
            humanId => {
                this.onChanged.emit(humanId);
            },
            response => {
                this.onError.emit(response);
            }
        )
    }
}
