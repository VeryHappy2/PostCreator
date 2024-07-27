import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { HttpService } from '../../../../services/http.service';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { IPostItem } from '../../../../models/enities/PostItem';
import { postUrl } from '../../../../urls';
import { take } from 'rxjs/internal/operators/take';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.scss'
})
export class UserItemComponent implements OnInit {
  public posts?: Array<IPostItem>
  public userName?: string
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpService) { }

  ngOnInit(): void {
    this.route.params.pipe(take(1)).subscribe((value: Params) => {
      let userId: IByIdRequest<string> = {
        id: value['id']
      }
      this.loadUser(userId)
    })
  }

  public toPostPage(id: number): void {
    this.router.navigate([`post/${id}`], { replaceUrl: true })
  }

  private loadUser(id: IByIdRequest<string>) {
    this.http
      .post<IByIdRequest<string>, IGeneralResponse<Array<IPostItem>>>(`${postUrl}/postbff/getpostsbyuserid`, id)
      .pipe(take(1))
      .subscribe(resp => 
        this.posts = resp.data)
  }
}
