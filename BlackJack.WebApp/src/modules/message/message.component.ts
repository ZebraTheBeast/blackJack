import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { MessageService } from 'src/services/message.service';

@Component({
    selector: 'app-error-message',
    templateUrl: '/src/modules/message/message.component.html',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css']
})

export class MessageComponent implements OnInit {

    @ViewChild('content') content: ElementRef;
    
    constructor(public messageService: MessageService) { }

    ngOnInit() {
        this.messageService.content = this.content;
    }
}
