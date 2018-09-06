import { NgModule } from '@angular/core';
import { SharedModule } from 'src/modules/shared.module';

import { MessageComponent } from 'src/modules/message/message.component';

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