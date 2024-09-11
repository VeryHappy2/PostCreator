import { Component, Input } from '@angular/core';
import { IPostItem } from '../../../../models/enities/PostItem';
import { Router } from '@angular/router';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { take } from 'rxjs/internal/operators/take';
import { PostManagmentService } from '../../services/post-managment.service';

@Component({
  selector: 'app-user-post',
  templateUrl: './user-post.component.html',
  styleUrl: './user-post.component.scss'
})

export class UserPostComponent {
  @Input() post?: IPostItem;

  constructor(
    private http: PostManagmentService,
    private router: Router) { }

  protected deletePost(): void {
    const request: IByIdRequest<number> = {
      id: this.post?.id!
    }

    this.http.deletePost(request)
      .pipe(take(1))
      .subscribe(
        resp => console.log(resp.message))
  }

  protected toPostPage(): void {
    this.router.navigate([`post/${this.post?.id}`], { replaceUrl: true })
  }
}
