import { NgModule } from '@angular/core';
import { SharedModule } from 'src/shared.module';
import { LoginRoutingModule } from 'src/login/login-routing.module';

import { LoginComponent } from 'src/login/login.component';

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