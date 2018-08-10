import { Game } from '../game/models/game';
import { Player } from '../game/models/Player';
import { Card } from '../game/models/card';
import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { GameService } from '../game/game.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-errorMessage',
    templateUrl: './error.component.html',
    styleUrls: ['../../../Content/bootstrap.css', '../../../Content/Site.css']
})
export class ErrorComponent {

    errorMessage: any;

    @ViewChild('content') content: ElementRef;

    constructor(private modalService: NgbModal) { }

    showError(response: any): void {
        this.errorMessage = response.error.Message;
        this.modalService.open(this.content);
    }
}
