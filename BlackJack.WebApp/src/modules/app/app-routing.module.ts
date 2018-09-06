import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'game/:id', loadChildren: 'src/modules/game/game.module#GameModule' },
    { path: 'login', loadChildren: 'src/modules/login/login.module#LoginModule' },
    { path: 'log', loadChildren: 'src/modules/log/log.module#LogModule' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule]
})

export class AppRoutingModule { }