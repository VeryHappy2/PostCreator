import { Injectable } from '@angular/core';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';
import { SessionSearchService } from '../../../services/session/session-search.service';
import { ISearchAdminUserResponse } from '../../../models/reponses/SearchAdminUserResponse';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { identityServerUrl } from '../../../urls';
import { HttpService } from '../../../services/http.service';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(
    private http: HttpService,
    private sessionSearchService: SessionSearchService) { }

  public searchByNameAdmin(userName: IByNameRequest<string | null> ): Observable<Array<ISearchAdminUserResponse>> {
    return this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchAdminUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameadmin`, userName)
      .pipe(
        map(response => {
          const users = response.data || [];
          this.sessionSearchService.saveData<Array<ISearchAdminUserResponse>>(`adminsearch${userName.name}`, users);
          return users
        }))
  }
}
