import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../services/http.service';
import { PageItemRequest,  } from '../../../../general-models/requests/PageItemRequest';
import { PageEvent } from '@angular/material/paginator';
import { Category } from '../../../../general-models/Category';
import { post } from '../../../../urls';
import { Observable } from 'rxjs';
import { MatSelectChange } from '@angular/material/select';
import { PaginatedItemsResponse } from '../../../../general-models/reponses/PaginatedItemsResponse';
import { PostItem } from '../../../../general-models/enities/PostItem';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {  
  public categories$!: Observable<Array<Category>>
  public pageItemResponse$?: Observable<PaginatedItemsResponse<PostItem>>

  private postPage!: PageItemRequest
  constructor(private http: HttpService, 
    private router: Router,) { }

  ngOnInit (): void {
    this.categories$ = this.http.get<Array<Category>>(`${post}/postbff/getpostcategories`)
  }

  onPageChange (eventPaginator: PageEvent): void {
    this.postPage.PageIndex = eventPaginator.pageIndex;
    this.postPage.PageSize = eventPaginator.pageSize;

    this.pageItemResponse$ = this.http.post<PageItemRequest, PaginatedItemsResponse<PostItem>>(`${post}/postbff/getpostsbypage`, this.postPage)
  }

  onSelectChange(eventSelect: MatSelectChange): void {
    this.postPage.CategoryFilter = eventSelect.value
    this.pageItemResponse$ = this.http.post<PageItemRequest, PaginatedItemsResponse<PostItem>>(`${post}/postbff/getpostsbypage`, this.postPage)
  }

  detailsPost(id: number) {
    this.router.navigate([`post/${id}`], { replaceUrl: true});
  }
}
