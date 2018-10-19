import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'game', loadChildren: 'src/app/game/game.module#GameModule' },
    { path: 'login', loadChildren: 'src/app/login/login.module#LoginModule' },
    { path: 'log', loadChildren: 'src/app/log/log.module#LogModule' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule]
})

export class AppRoutingModule { }