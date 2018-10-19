import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { MessageComponent } from 'src/app/message/message/message.component';

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