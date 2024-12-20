import { TestBed } from '@angular/core/testing';

import { SearchService } from './search.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SessionSearchService } from '../../../services/session/session-search.service';
import { HttpService } from '../../../services/http.service';
import { IGeneralResponse } from '../../../models/responses/GeneralResponse';
import { IPostItem } from '../../../models/entities/PostItem';
import { IPaginatedItemsResponse } from '../../../models/responses/PaginatedItemsResponse';
import { of } from 'rxjs/internal/observable/of';
import { IPageItemsRequest } from '../../../models/requests/PageItemRequest';
import { postUrl } from '../../../../env/urls';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';

describe('SearchService', () => {
  let service: SearchService;
  let searchService: SessionSearchService
  let http: HttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        SessionSearchService,
        HttpService,
        SearchService
      ]
    });

    http = TestBed.inject(HttpService); 
    searchService = TestBed.inject(SessionSearchService)
    service = TestBed.inject(SearchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get the posts', () => {
    const response: IGeneralResponse<IPaginatedItemsResponse<IPostItem>> = {
      flag: true,
      data: {
        pageIndex: 1,
        data: [
          {
            userName: "name",
            id: 1,
            title: "title",
            category: {
              category: "category",
              id: 1
            },
            comments: [],
            content: "content",
            date: "date",
            userId: "userid",
            views: 2,
            likes: 1
          },
        ],
        count: 1,
        pageSize: 1,
        search: ""
      },
      message: "message"
    };

    const request: IPageItemsRequest = {
      pageIndex: 1,
      pageSize: 1,
      categoryFilter: 1,
      searchByTitle: "title",
      searchByUserName: "name",
    };

    spyOn(http, "post").and.returnValue(of(response));

    service.searchPostsByPage(request).subscribe(resp => {
      expect(http.post).toHaveBeenCalledOnceWith(`${postUrl}/postbff/getpostsbypage`, request)
      expect(resp).toEqual(response);
    })
  });

  it("should search the post by id", () => {
    const request: IByIdRequest<number> = {
      id: 1
    }

    const response: IGeneralResponse<IPostItem> = {
      flag: true,
      message: "message",
      data: {
        userName: "name",
        id: 1,
        title: "title",
        category: {
          category: "category",
          id: 1
        },
        comments: [],
        content: "content",
        date: "date",
        userId: "userid",
        views: 2,
        likes: 2
      },
    } 
    spyOn(http, "post").and.returnValue(of(response));

    service.searchPostById(request).subscribe(respHttp => {
      expect(http.post).toHaveBeenCalledOnceWith(`${postUrl}/postItem/getbyId`, request)
      expect(respHttp).toEqual(response);
    })
  });
});
