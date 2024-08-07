import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NoPageComponent } from './components/no-page/no-page.component';
import { ErrorComponent } from './components/error/error.component';

const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    redirectTo: "home",
  },
  {
    path: "home",
    loadChildren: () => import('./modules/home/home.module').then((m) => m.HomeModule)
  },
  {
    path: "auth",
    loadChildren: () => import("./modules/auth/auth.module").then((m) => m.AuthModule)
  },
  {
    path: "post",
    loadChildren: () => import("./modules/post/post.module").then((m) => m.PostModule),
  },
  {
    path: "user",
    loadChildren: () => import("./modules/user/user.module").then((m) => m.UserModule),
  },
  {
    path: "admin",
    loadChildren: () => import("./modules/admin/admin.module").then((m) => m.AdminModule),
  },
  {
    path: "error",
    component: ErrorComponent
  },
  {
    path: "**",
    component: NoPageComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule 
{ }
