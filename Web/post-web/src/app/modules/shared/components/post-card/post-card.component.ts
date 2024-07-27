import { Component, Input } from '@angular/core';
import { IPostItem } from '../../../../models/enities/PostItem';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrl: './post-card.component.scss'
})
export class PostCardComponent {
  @Input() post!: IPostItem

  constructor(private router: Router) { }

  public detailsPost(id: number) {
    this.router.navigate([`post/${id}`], { replaceUrl: true });
  }

  public toUserPage(id: string) {
    this.router.navigate([`user/${id}`], { replaceUrl: true });
  }
}
