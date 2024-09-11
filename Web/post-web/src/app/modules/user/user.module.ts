import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UserRoutingModule } from './user-routing.module';
import { MatSelectModule } from '@angular/material/select';
import { UserItemComponent } from './components/user-item/user-item.component';
import { UserPostComponent } from './components/user-post/user-post.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from '../shared/shared.module';
import { UserService } from './services/user.service';
import { PostManagmentService } from './services/post-managment.service';

@NgModule({
  declarations: [
    DashboardComponent,
    UserItemComponent,
    UserPostComponent,
  ],
  imports: [
    MatSelectModule,
    CommonModule,
    UserRoutingModule,
    MatIconModule,
    MatButtonModule,
    SharedModule
  ],
  providers: [
    UserService,
    PostManagmentService
  ]
})
export class UserModule { }
