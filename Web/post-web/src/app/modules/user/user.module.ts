import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { MenuComponent } from './components/menu/menu.component';
import { PostsComponent } from './components/dashboard/user-post/user-post.component';
import { UserRoutingModule } from './user-routing.module';
import { MatSelectModule } from '@angular/material/select';
import { UserItemComponent } from './components/user-item/user-item.component';



@NgModule({
  declarations: [
    DashboardComponent,
    MenuComponent,
    PostsComponent,
    UserItemComponent
  ],
  exports: [
    MenuComponent
  ],
  imports: [
    MatSelectModule,
    CommonModule,
    UserRoutingModule,
  ]
})
export class UserModule { }
