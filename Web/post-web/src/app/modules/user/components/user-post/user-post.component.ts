import { Component, Input } from '@angular/core';
import { HttpService } from '../../../../services/http.service';
import { IPostItem } from '../../../../models/enities/PostItem';
import { postUrl } from '../../../../urls';
import { Router } from '@angular/router';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { take } from 'rxjs/internal/operators/take';

@Component({
  selector: 'app-user-post',
  templateUrl: './user-post.component.html',
  styleUrl: './user-post.component.scss'
})

export class UserPostComponent {
  @Input() post?: IPostItem;

  constructor(
    private http: HttpService,
    private router: Router) { }

  protected deletePost(): void {
    let request: IByIdRequest<number> = {
      id: this.post?.id!
    }
    this.http.post<IByIdRequest<number>, IGeneralResponse<number>>(`${postUrl}/postitem/delete`, request)
    .pipe(take(1))
    .subscribe(
      resp => console.log(resp.message))
  }

  protected toPostPage(): void {
    this.router.navigate([`post/${this.post?.id}`], { replaceUrl: true })
  }
}
