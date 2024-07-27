import { NgModule } from '@angular/core';
import { PostListComponent } from './components/post-list/post-list.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { PostItemComponent } from './components/post-item/post-item.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { PostRoutingModule } from './post-routing.module';
import { PostCreateComponent } from './components/post-create/post-create.component';
import { UserModule } from '../user/user.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { AutoResizeTextareaDirective } from './directives/auto-resize-textarea.directive';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    PostListComponent,
    PostItemComponent,
    AutoResizeTextareaDirective,
    PostCreateComponent
  ],
  imports: [
    CommonModule,
    MatPaginatorModule,
    MatGridListModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    PostRoutingModule,
    UserModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatIconModule,
    MatButtonModule,
    SharedModule
  ],
  exports: [
    SharedModule
  ]
})
export class PostModule { }