import { NgModule } from '@angular/core';
import { SharedModule } from 'src/shared.module';

import { MessageComponent } from 'src/message/message.component';

@NgModule({
    imports: [
        SharedModule
    ],
    declarations: [
        MessageComponent
    ],
    exports: [
        MessageComponent
    ]
})

export class MessageModule { }