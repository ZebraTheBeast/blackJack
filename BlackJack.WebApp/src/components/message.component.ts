import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { MessageService } from '../services/message.service';

@Component({
    selector: 'app-error-message',
    templateUrl: '../views/message.component.html',
    styleUrls: ['../../Content/bootstrap.css', '../../Content/Site.css']
})

export class MessageComponent implements OnInit {

    @ViewChild('content') content: ElementRef;
    
    constructor(public messageService: MessageService) { }

    ngOnInit() {
        this.messageService.content = this.content;
    }
}
