import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WelcomeComponent } from './welcome.component';
import { HomeComponent } from './home.component';

const routes: Routes = [
  { path: '', redirectTo: '/welcome', pathMatch: "full" },
  { path: 'welcome', component: WelcomeComponent, pathMatch: "full" },
  { path: 'home', component: HomeComponent, pathMatch: "full" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
