import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from '../../components/app.component';
import { MessageComponent } from '../../components/message.component';
import { GameModule } from '../game/game.module';
import { LogModule } from '../log/log.module';
import { LoginModule } from '../login/login.module';



@NgModule({
    declarations: [
        AppComponent,
        MessageComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        AppRoutingModule,
        NgbModule.forRoot(),
        BrowserAnimationsModule,
        GameModule,
        LogModule,
        LoginModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})

export class AppModule { }