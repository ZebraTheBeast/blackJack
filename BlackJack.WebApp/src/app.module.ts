import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from 'src/app.component';
import { MessageComponent } from 'src/components/message.component';
import { AppRoutingModule } from 'src/modules/app/app-routing.module';
import { GameModule } from 'src/modules/game/game.module';
import { LogModule } from 'src/modules/log/log.module';
import { LoginModule } from './modules/login/login.module';

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