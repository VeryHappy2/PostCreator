import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpService } from '../../../../services/http.service';
import { PostItem } from '../../../../models/enities/PostItem';
import { ByIdRequest } from '../../../../models/requests/ByIdRequest';
import { postUrl } from '../../../../urls';
import { JsonPipe } from '@angular/common';
import { GeneralResponse } from '../../../../models/reponses/GeneralResponse';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.scss'
})
export class PostItemComponent implements OnInit {
  public post$?: Observable<GeneralResponse<PostItem>>

  constructor(
    private route: ActivatedRoute,
    private http: HttpService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((p: Params) => {
      let id: ByIdRequest<number> = {
        id: +p['id']
      };
      if (id) {
        this.post$ = this.http.post<ByIdRequest<number>, GeneralResponse<PostItem>>(`${postUrl}/postbff/getpostbyId`, id)
      
        this.post$.subscribe((value: GeneralResponse<PostItem>) => {
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

  public back() {
    this.router.navigate([`post/post-list`])
  }
}
