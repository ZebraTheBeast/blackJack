import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Injectable({ providedIn: 'root' })
export class MessageService {
    errorMessage: any;
    content: any;

    constructor(private modalService: NgbModal) { }

    showError(message: any): void {
        this.errorMessage = message.error.Message;
        this.modalService.open(this.content);
    }
}
