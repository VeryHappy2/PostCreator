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
import { GeneralResponse } from '../../../../models/reponses/GeneralResponse';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {  
  public categories$!: Observable<GeneralResponse<Array<PostCategory>>>
  public pageItemResponse!: GeneralResponse<PaginatedItemsResponse<PostItem>>;

  private postPageRequest!: PageItemRequest

  constructor(
    private http: HttpService, 
    private router: Router,) {
    this.postPageRequest = {
      PageIndex: 0,
      PageSize: 10 
    } 
  }

  ngOnInit (): void {
    this.categories$ = this.http.get<GeneralResponse<Array<PostCategory>>>(`${postUrl}/postbff/getpostcategories`)

    this.loadPosts()
  }

  public onPageChange (eventPaginator: PageEvent): void {
    this.postPageRequest.PageIndex = eventPaginator.pageIndex;
    this.postPageRequest.PageSize = eventPaginator.pageSize;
    
    this.loadPosts()
  }

  public onSelectChange(eventSelect: MatSelectChange): void {
    this.postPageRequest.CategoryFilter = eventSelect.value
    this.loadPosts()
  }

  public detailsPost(id: number) {
    this.router.navigate([`post/${id}`], { replaceUrl: true});
  }

  private loadPosts(): void {
    this.http.post<PageItemRequest, GeneralResponse<PaginatedItemsResponse<PostItem>>>(`${postUrl}/postbff/getpostsbypage`, this.postPageRequest)
      .subscribe((response: GeneralResponse<PaginatedItemsResponse<PostItem>>) => this.pageItemResponse = response);
  }
}
