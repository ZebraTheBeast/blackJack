import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from 'src/app.component';

import { AppRoutingModule } from 'src/app-routing.module';
import { GameModule } from 'src/game/game.module';
import { LogModule } from 'src/log/log.module';
import { LoginModule } from 'src/login/login.module';
import { MessageModule } from 'src/message/message.module';

@NgModule({
    declarations: [
        AppComponent
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
        LoginModule,
        MessageModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})

export class AppModule { }