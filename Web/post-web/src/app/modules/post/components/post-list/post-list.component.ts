import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../services/http.service';
import { PageEvent } from '@angular/material/paginator';
import { postUrl } from '../../../../urls';
import { MatSelectChange } from '@angular/material/select';
import { IPostItem } from '../../../../models/enities/PostItem';
import { Router } from '@angular/router';
import { IPaginatedItemsResponse } from '../../../../models/reponses/PaginatedItemsResponse';
import { IPageItemsRequest } from '../../../../models/requests/PageItemRequest';
import { IPostCategory } from '../../../../models/enities/PostCategory';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs/internal/Observable';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {
  protected searchByTitle = new FormControl('')
  protected categories$!: Observable<IGeneralResponse<Array<IPostCategory>>>
  protected pageItemResponse$!: Observable<IGeneralResponse<IPaginatedItemsResponse<IPostItem>>>;

  private postPageRequest!: IPageItemsRequest

  constructor(
    private http: HttpService, 
    private router: Router,) {
    this.postPageRequest = {
      pageIndex: 0,
      pageSize: 10 
    } 
  }

  ngOnInit (): void {
    this.categories$ = this.http.get<IGeneralResponse<Array<IPostCategory>>>(`${postUrl}/postbff/getpostcategories`)
    this.loadPosts()
  }

  protected onUserNameChange(event: string): void {
    this.postPageRequest.searchByUserName = event

    this.loadPosts()
  }

  protected onSearchByTitle(): void {
    this.postPageRequest.searchByTitle = this.searchByTitle.value

    this.loadPosts()
  }

  protected onPageChange (eventPaginator: PageEvent): void {
    this.postPageRequest.pageIndex = eventPaginator.pageIndex;
    this.postPageRequest.pageSize = eventPaginator.pageSize;
    
    this.loadPosts()
  }

  protected onSelectChange(eventSelect: MatSelectChange): void {
    this.postPageRequest.categoryFilter = eventSelect.value
    
    this.loadPosts()
  }

  protected detailsPost(id: number): void {
    this.router.navigate([`post/${id}`], { replaceUrl: true });
  }

  protected loadPosts(): void {
    this.pageItemResponse$ = this.http.post<IPageItemsRequest, IGeneralResponse<IPaginatedItemsResponse<IPostItem>>>(`${postUrl}/postbff/getpostsbypage`, this.postPageRequest);
  }
}
