import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';
import { IPostItem } from '../../../../models/entities/PostItem';
import { take } from 'rxjs/internal/operators/take';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.scss'
})
export class UserItemComponent implements OnInit {
  protected posts?: Array<IPostItem>
  protected userName?: string
  
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private user: UserService) { }

  ngOnInit(): void {
    this.route.params.pipe(take(1)).subscribe((value: Params) => {
      let userId: IByIdRequest<string> = {
        id: value['id']
      }
      this.loadUser(userId);
    })
  }

  protected toPostPage(id: number): void {
    this.router.navigate([`post/${id}`], { replaceUrl: true })
  }

  private loadUser(id: IByIdRequest<string>): void {
    this.user.getPostsByUserId(id)
      .pipe(take(1))
      .subscribe(resp => this.posts = resp.data)
  }
}
