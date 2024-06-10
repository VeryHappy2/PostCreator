import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MenuComponent } from './components/menu/menu.component';
import { PostsComponent } from './components/dashboard/posts/posts.component';
import { UserRoutingModule } from './user-routing.module';



@NgModule({
  declarations: [
    DashboardComponent,
    MenuComponent,
    PostsComponent
  ],
  exports: [
    MenuComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
  ]
})
export class UserModule { }
