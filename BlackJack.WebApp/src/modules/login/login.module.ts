import { NgModule } from '@angular/core';
import { SharedModule } from 'src/modules/shared.module';
import { LoginRoutingModule } from 'src/modules/login/login-routing.module';

import { LoginComponent } from 'src/modules/login/login.component';

@NgModule({
    imports: [
        SharedModule,
        LoginRoutingModule
    ],
    declarations: [
        LoginComponent
    ]
})

export class LoginModule { }