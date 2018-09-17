﻿import { Component } from '@angular/core';

import { State, process } from '@progress/kendo-data-query';
import { GridDataResult, DataStateChangeEvent } from '@progress/kendo-angular-grid';

import { LogService } from 'src/log/log.service';
import { MessageService } from 'src/message/message.service';

@Component({
    selector: 'app-log',
    styleUrls: ['/Content/bootstrap.css', '/Content/Site.css'],
    templateUrl: '/src/log/log.component.html'
})

export class LogComponent {
    gridData: GridDataResult;
    data: any[];
    state: State = {
        skip: 0,
        take: 12,
    };

    constructor(private logService: LogService, private messageService: MessageService) { }

    ngOnInit() {
        this.loadLogs();
    }

    public dataStateChange(state: DataStateChangeEvent): void {
        this.state = state;
        this.gridData = process(this.data, this.state);
    }

    loadLogs(): void {
        this.logService.loadLogs().subscribe(
            data => {
                this.data = data;
                this.data.forEach(item => item.CreationDate = new Date(item.CreationDate));
                this.gridData = process(this.data, this.state);
            },
            response => {
                this.messageService.showError(response);
            }
        )
    
    }
}
