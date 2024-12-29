import { AfterViewChecked, AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { IPostItem } from '../../../../models/entities/PostItem';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { IGeneralResponse as IGeneralResponse } from '../../../../models/responses/GeneralResponse';
import { FormControl} from '@angular/forms';
import { IPostComment } from '../../../../models/entities/PostComment';
import { IPostCommentRequest } from '../../../../models/requests/CommentRequest';
import { Observable } from 'rxjs/internal/Observable';
import { take } from 'rxjs/internal/operators/take';
import { ManagementService } from '../../services/management.service';
import { SearchService } from '../../services/search.service';
import { HttpErrorResponse } from '@angular/common/http';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.scss'
})
export class PostItemComponent implements OnInit, AfterViewInit {
  @ViewChild('content', { static: false }) contentElement!: ElementRef;
  public commentCtrl = new FormControl('')
  private postSubject = new BehaviorSubject<any>(null);
  public post$?: Observable<IGeneralResponse<IPostItem>> = this.postSubject.asObservable()
  public success?: boolean
  public id?: IByIdRequest<number>
  public nonSuccessMessageLike?: string
  private timeForReading?: number
  private timeStarted: boolean = false
  private postId!: number
  timer: any;

  constructor(
    private route: ActivatedRoute,
    private managementService: ManagementService,
    private searchService: SearchService,
    private router: Router,
    private token: TokenStorageService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((p: Params) => {
      this.id = {
        id: +p['id']
      };

      if (this.id) {
        this.post$ = this.searchService.searchPostById(this.id)

        this.post$
          .subscribe({
            next: (resp) => {
              const wordsPerMinute = 200
              this.timeForReading = (resp.data?.content.split(/\s+/).length! / wordsPerMinute) * 60000
            },
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

  ngAfterViewInit(): void {
      setTimeout(() => {
        const observer = new IntersectionObserver((entries) => {
          entries.forEach(entry => {
            if (entry.isIntersecting && !this.timeStarted) {
              this.startTimer(observer);
            }
          });
        });

        if (this.contentElement) {
          observer.observe(this.contentElement.nativeElement);
        }
      }, 120);
  }

  startTimer(observer: IntersectionObserver) {
    if (!this.timer) {
      this.timer = setTimeout(() => {
        this.managementService.addView(this.id!)
          .subscribe({
            next: (resp) => {
              console.log(resp.message);
              observer.disconnect();
            },
        error: (err) => console.error('Error:', err)
        });
      }, this.timeForReading);
    }
  }

  protected addComment(): void {
    this.route.params.subscribe((p: Params) => {
      this.postId = +p['id']
    })
    if (this.commentCtrl.value) {
      const comment: IPostCommentRequest = {
        postId: this.postId,
        content: this.commentCtrl.value,
      }

      this.managementService.addComment(comment)
        .pipe(take(1))
        .subscribe({
          next: resp => {
            this.success = resp.flag
            this.commentCtrl.reset()
            const updatedPost: IGeneralResponse<IPostItem> = {
              ...this.postSubject.getValue(),
              comments: [...(this.postSubject.getValue().comments! || []), {
                id: resp.data,
                postId: this.postId,
                content: comment.content,
                userName: this.token.getUsername()
              } as IPostComment]
            }
            this.postSubject.next(updatedPost)
          },
          error: () => this.success = false
        })
    }
  }

  public addLike(): void {
    if (this.token.getUsername()) {
      this.managementService.addLike(this.id!).subscribe(
        {
          next: (value) => {
            console.log(value.message)
          },
          error: (err: HttpErrorResponse) => {
            this.nonSuccessMessageLike = err.error.message
          }
        })
    } else {
      this.nonSuccessMessageLike = "You need to log in"
    }
  }

  protected back() {
    this.router.navigate([`post/post-list`])
  }
}
