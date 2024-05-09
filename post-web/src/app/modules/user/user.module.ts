import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MenuComponent } from './components/menu/menu.component';
import { PostsComponent } from './components/dashboard/posts/posts.component';



@NgModule({
  declarations: [
    DashboardComponent,
    MenuComponent,
    PostsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class UserModule { }
