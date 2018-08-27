import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from '../components/login.component';
import { LoginRoutingModule } from './login-routing.module';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        LoginRoutingModule
    ],
    declarations: [
        LoginComponent
    ]
})

export class LoginModule { }