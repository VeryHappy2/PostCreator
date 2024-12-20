import { Component, Input } from '@angular/core';
import { IPostItem } from '../../../../models/entities/PostItem';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrl: './post-card.component.scss'
})
export class PostCardComponent {
  @Input() post!: IPostItem

  constructor(private router: Router) { }

  protected detailsPost(id: number): void {
    this.router.navigate([`post/${id}`], { replaceUrl: true });
  }

  protected toUserPage(id: string): void {
    this.router.navigate([`user/${id}`], { replaceUrl: true });
  }
}
