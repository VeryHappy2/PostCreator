import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../services/http.service';
import { PageEvent } from '@angular/material/paginator';
import { identityServerUrl, postUrl } from '../../../../urls';
import { map, Observable } from 'rxjs';
import { MatSelectChange } from '@angular/material/select';
import { IPostItem } from '../../../../models/enities/PostItem';
import { Router } from '@angular/router';
import { IPaginatedItemsResponse } from '../../../../models/reponses/PaginatedItemsResponse';
import { IPageItemsRequest } from '../../../../models/requests/PageItemRequest';
import { IPostCategory } from '../../../../models/enities/PostCategory';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { FormControl } from '@angular/forms';
import { ISearchAdminUserResponse } from '../../../../models/reponses/SearchAdminUserResponse';
import { IByNameRequest } from '../../../../models/requests/user/ByNameRequest';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {
  public filteredUsers?: Observable<Array<ISearchAdminUserResponse>>
  public searchByTitle = new FormControl('')
  public searchByUserName = new FormControl('')
  public userCtrl = new FormControl('')
  public categories$!: Observable<IGeneralResponse<Array<IPostCategory>>>
  public pageItemResponse$!: Observable<IGeneralResponse<IPaginatedItemsResponse<IPostItem>>>;

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
    this.userCtrl.valueChanges
      .subscribe(value => {
        this.onUserNameChange(value);
      });
    this.loadPosts()
  }

  public onSearchByTitle() {
    this.postPageRequest.searchByTitle = this.searchByTitle.value

    this.loadPosts()
  }

  public onPageChange (eventPaginator: PageEvent): void {
    this.postPageRequest.pageIndex = eventPaginator.pageIndex;
    this.postPageRequest.pageSize = eventPaginator.pageSize;
    
    this.loadPosts()
  }

  public onSelectChange(eventSelect: MatSelectChange): void {
    this.postPageRequest.categoryFilter = eventSelect.value
    
    this.loadPosts()
  }

  public detailsPost(id: number) {
    this.router.navigate([`post/${id}`], { replaceUrl: true });
  }

  private onUserNameChange(userName: string | null) {
    const request: IByNameRequest<string | null> = {
      name: userName
    }
    this.filteredUsers = this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchAdminUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameuser`, request).pipe(
      map(response => response.data || [])
    );
    this.postPageRequest.searchByUserName = userName;
    this.loadPosts()
  }

  private loadPosts(): void {
    this.pageItemResponse$ = this.http.post<IPageItemsRequest, IGeneralResponse<IPaginatedItemsResponse<IPostItem>>>(`${postUrl}/postbff/getpostsbypage`, this.postPageRequest);
  }
}
