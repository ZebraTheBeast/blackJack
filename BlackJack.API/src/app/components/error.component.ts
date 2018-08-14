import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-error-message',
    templateUrl: '../views/error.component.html',
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
