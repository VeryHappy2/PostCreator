import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { IGeneralResponse } from '../../../models/responses/GeneralResponse';
import { IPaginatedItemsResponse } from '../../../models/responses/PaginatedItemsResponse';
import { IPostItem } from '../../../models/entities/PostItem';
import { identityServerUrl, postUrl } from '../../../../env/urls';
import { IPageItemsRequest } from '../../../models/requests/PageItemRequest';
import { map, Observable } from 'rxjs';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';
import { SessionSearchService } from '../../../services/session/session-search.service';
import { ISearchUserResponse } from '../../../models/responses/SearchUserResponse';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';

@Injectable()
export class SearchService {

  constructor(
    private http: HttpService,
    private sessionService: SessionSearchService) { }

  public searchPostsByPage(request: IPageItemsRequest): Observable<IGeneralResponse<IPaginatedItemsResponse<IPostItem>>> {
    return this.http.post(`${postUrl}/postbff/getpostsbypage`, request)
  }

  public searchUsersByName(request: IByNameRequest<string | null>)  {
    return this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameuser`, request).pipe(
      map(response => {
        const users = response.data || [];
        this.sessionService.saveData(request.name!, users);
        return users;
      }))
  }

  public searchPostById(request: IByIdRequest<number>) {
    return this.http.post<IByIdRequest<number>, IGeneralResponse<IPostItem>>(`${postUrl}/postItem/getbyId`, request)
  }
}
