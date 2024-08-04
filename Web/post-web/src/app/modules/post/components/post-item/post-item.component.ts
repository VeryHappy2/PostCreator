import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { HttpService } from '../../../../services/http.service';
import { IPostItem } from '../../../../models/enities/PostItem';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { postUrl } from '../../../../urls';
import { IGeneralResponse as IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { FormControl} from '@angular/forms';
import { IPostCommentRequest } from '../../../../models/requests/CommentRequest';
import { Observable } from 'rxjs/internal/Observable';
import { take } from 'rxjs/internal/operators/take';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.scss'
})
export class PostItemComponent implements OnInit {
  protected commentCtrl = new FormControl('')
  protected post$?: Observable<IGeneralResponse<IPostItem>>

  private postId!: number
  constructor(
    private route: ActivatedRoute,
    private http: HttpService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((p: Params) => {
      let id: IByIdRequest<number> = {
        id: +p['id']
      };

      if (id) {
        this.post$ = this.http.post<IByIdRequest<number>, IGeneralResponse<IPostItem>>(`${postUrl}/postbff/getpostbyId`, id)
      
        this.post$.subscribe((value: IGeneralResponse<IPostItem>) => {
          if (!value) {
            this.router.navigate(['no-page'])
          }
        })
      }
      else {
        this.router.navigate(['no-page'])
      }
    });
  }

  protected addComment(): void {
    this.route.params.subscribe((p: Params) => {
      this.postId = +p['id']
    })
    if (this.commentCtrl.value) {
      let comment: IPostCommentRequest = {
        postId: this.postId,
        content: this.commentCtrl.value
      }

      this.http.post<IPostCommentRequest, IGeneralResponse<number>>(`${postUrl}/postcomment/add`, comment).pipe(take(1)).subscribe(resp => console.log(resp.message))
    }
  }

  protected back() {
    this.router.navigate([`post/post-list`])
  }
}
