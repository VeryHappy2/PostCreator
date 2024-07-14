import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostListComponent } from './components/post-list/post-list.component';
import { PostItemComponent } from './components/post-item/post-item.component';
import { PostCreateComponent } from './components/post-create/post-create.component';
import { AuthUserGuardService } from '../../services/auth/auth-user-guard.service';

const routes: Routes = [
  {
    path: "post-list",
    component: PostListComponent
  },
  {
    path: "post-create",
    component: PostCreateComponent,
    canActivate: [AuthUserGuardService]
  },
  {
    path: ":id",
    component: PostItemComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PostRoutingModule { }
