import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../services/http.service';
import { PageEvent } from '@angular/material/paginator';
import { postUrl } from '../../../../../env/urls';
import { MatSelectChange } from '@angular/material/select';
import { IPostItem } from '../../../../models/entities/PostItem';
import { Router } from '@angular/router';
import { IPaginatedItemsResponse } from '../../../../models/responses/PaginatedItemsResponse';
import { IPageItemsRequest } from '../../../../models/requests/PageItemRequest';
import { IPostCategory } from '../../../../models/entities/PostCategory';
import { IGeneralResponse } from '../../../../models/responses/GeneralResponse';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs/internal/Observable';
import { SessionService } from '../../../../services/session/session.service';

const PAGE_KEY = "PAGE_KEY"

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
    private router: Router,
    private sessionStorage: SessionService) {
    this.postPageRequest = {
      pageIndex: 0,
      pageSize: 10,
      searchByUserName: "",
      searchByTitle: "",
      categoryFilter: null
    } 
  }

  ngOnInit (): void {
    this.categories$ =
      this.http.get<IGeneralResponse<Array<IPostCategory>>>(`${postUrl}/postbff/getpostcategories`)
    const responsePage: IPageItemsRequest = this.sessionStorage.getData<IPageItemsRequest>(PAGE_KEY)!

    if (responsePage) {
      this.postPageRequest = responsePage
    }
    
    this.loadPosts()
  }

  protected onUserNameChange(event: string): void {
    if (this.postPageRequest.searchByUserName !== event) {
      this.postPageRequest.searchByUserName = event;
      this.loadPosts();
    }
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
    this.sessionStorage.saveData<IPageItemsRequest>(PAGE_KEY, this.postPageRequest)

    this.router.navigate([`post/${id}`], { replaceUrl: true });
  }

  protected loadPosts(): void {
    console.log('loadPosts called');
    this.pageItemResponse$ = this.http.post<IPageItemsRequest, IGeneralResponse<IPaginatedItemsResponse<IPostItem>>>(`${postUrl}/postbff/getpostsbypage`, this.postPageRequest);
    this.pageItemResponse$.subscribe(resp => console.log(JSON.stringify(resp)))
  }
}
