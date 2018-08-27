import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { GameModule } from './game.module';

import { AppComponent } from '../components/app.component';
import { LoginComponent } from '../components/login.component';
import { LogComponent } from '../components/log.component';
import { MessageComponent } from '../components/message.component';
import { LogModule } from './log.module';
import { LoginModule } from './login.module';


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
        GridModule,
        BrowserAnimationsModule,
        GameModule,
        LogModule,
        LoginModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})

export class AppModule { }