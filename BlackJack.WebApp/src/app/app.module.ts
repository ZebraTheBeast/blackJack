import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { GameModule } from 'src/app/game/game.module';
import { LogModule } from 'src/app/log/log.module';
import { LoginModule } from 'src/app/login/login.module';
import { MessageModule } from 'src/app/message/message.module';

import { AppComponent } from 'src/app/app.component';

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