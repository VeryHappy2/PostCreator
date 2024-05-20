import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../services/http.service';
import { PageEvent } from '@angular/material/paginator';
import { postUrl } from '../../../../urls';
import { Observable } from 'rxjs';
import { MatSelectChange } from '@angular/material/select';
import { PostItem } from '../../../../models/enities/PostItem';
import { Router } from '@angular/router';
import { PaginatedItemsResponse } from '../../../../models/reponses/PaginatedItemsResponse';
import { PageItemRequest } from '../../../../models/requests/PageItemRequest';
import { PostCategory } from '../../../../models/enities/PostCategory';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {  
  public categories$!: Observable<Array<PostCategory>>
  public pageItemResponse$?: Observable<PaginatedItemsResponse<PostItem>>

  private postPage!: PageItemRequest
  constructor(private http: HttpService, 
    private router: Router,) { }

  ngOnInit (): void {
    this.categories$ = this.http.get<Array<PostCategory>>(`${postUrl}/postbff/getpostcategories`)
  }

  onPageChange (eventPaginator: PageEvent): void {
    this.postPage.PageIndex = eventPaginator.pageIndex;
    this.postPage.PageSize = eventPaginator.pageSize;

    this.pageItemResponse$ = this.http.post<PageItemRequest, PaginatedItemsResponse<PostItem>>(`${postUrl}/postbff/getpostsbypage`, this.postPage)
  }

  onSelectChange(eventSelect: MatSelectChange): void {
    this.postPage.CategoryFilter = eventSelect.value
    this.pageItemResponse$ = this.http.post<PageItemRequest, PaginatedItemsResponse<PostItem>>(`${postUrl}/postbff/getpostsbypage`, this.postPage)
  }

  detailsPost(id: number) {
    this.router.navigate([`post/${id}`], { replaceUrl: true});
  }
}
