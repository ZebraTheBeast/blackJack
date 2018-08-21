import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { GameComponent } from '../components/game.component';
import { LoginComponent } from '../components/login.component';
import { LogComponent } from '../components/log.component';

const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'game/:id', component: GameComponent },
    { path: 'login', component: LoginComponent },
    { path: 'log', component: LogComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule]
})

export class AppRoutingModule { }