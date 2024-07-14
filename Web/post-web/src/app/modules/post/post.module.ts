import { NgModule } from '@angular/core';
import { PostListComponent } from './components/post-list/post-list.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { PostItemComponent } from './components/post-item/post-item.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { PostRoutingModule } from './post-routing.module';
import { PostCreateComponent } from './components/post-create/post-create.component';
import { UserModule } from '../user/user.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    PostListComponent,
    PostItemComponent,
    PostCreateComponent,
  ],
  imports: [
    CommonModule,
    MatPaginatorModule,
    MatGridListModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatListModule,
    PostRoutingModule,
    UserModule,
    ReactiveFormsModule
  ]
})
export class PostModule { }