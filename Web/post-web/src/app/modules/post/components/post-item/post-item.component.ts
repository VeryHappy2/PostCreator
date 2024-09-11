import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { IPostItem } from '../../../../models/enities/PostItem';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { IGeneralResponse as IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { FormControl} from '@angular/forms';
import { IPostCommentRequest } from '../../../../models/requests/CommentRequest';
import { Observable } from 'rxjs/internal/Observable';
import { take } from 'rxjs/internal/operators/take';
import { ManagementService } from '../../services/management.service';
import { SearchService } from '../../services/search.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.scss'
})
export class PostItemComponent implements OnInit {
  protected commentCtrl = new FormControl('')
  protected post$?: Observable<IGeneralResponse<IPostItem>>
  protected successMessage?: boolean

  private postId!: number

  constructor(
    private route: ActivatedRoute,
    private managementService: ManagementService,
    private searchService: SearchService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((p: Params) => {
      const id: IByIdRequest<number> = {
        id: +p['id']
      };

      if (id) {
        this.post$ = this.searchService.searchPostById(id)
      
        this.post$
          .subscribe({
            error: (error: HttpErrorResponse) => {
              console.warn(error.message)
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
      const comment: IPostCommentRequest = {
        postId: this.postId,
        content: this.commentCtrl.value
      }

      this.managementService.addComment(comment)
        .pipe(take(1))
        .subscribe(resp => this.successMessage = resp.flag)
    }
  }

  protected back() {
    this.router.navigate([`post/post-list`])
  }
}
