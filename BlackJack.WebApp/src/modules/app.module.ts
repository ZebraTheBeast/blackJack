import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { GridModule } from '@progress/kendo-angular-grid';

import { AppComponent } from '../components/app.component';
import { GameComponent } from '../components/game.component';
import { LoginComponent } from '../components/login.component';
import { LogComponent } from '../components/log.component';
import { MessageComponent } from '../components/message.component';

@NgModule({
    declarations: [
        AppComponent,
        GameComponent,
        LoginComponent,
        MessageComponent,
        LogComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        AppRoutingModule,
        NgbModule.forRoot(),
        GridModule,
        BrowserAnimationsModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})

export class AppModule { }