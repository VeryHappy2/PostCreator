import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { PostItem } from '../../../../general-models/enities/PostItem';
import { HttpService } from '../../../../services/http.service';
import { post as urlPost } from '../../../../urls';
import { ByIdRequest } from '../../../../general-models/requests/ByIdRequest';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.scss'
})
export class PostItemComponent implements OnInit {
  public post$?: Observable<PostItem>

  constructor(
    private route: ActivatedRoute,
    private http: HttpService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((p: Params) => {
      let id: ByIdRequest<number> = {
        id: +p['id']
      };
      this.post$ = this.http.post<ByIdRequest<number>, PostItem>(`${urlPost}/postbff/getpostbyId`, id)
    });
  }
}
