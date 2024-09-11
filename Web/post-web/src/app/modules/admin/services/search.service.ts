import { Injectable } from '@angular/core';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';
import { ISearchAdminUserResponse } from '../../../models/reponses/SearchAdminUserResponse';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { identityServerUrl } from '../../../urls';
import { HttpService } from '../../../services/http.service';
import { map, Observable, of } from 'rxjs';
import { SessionSearchService } from '../../../services/session/session-search.service';

@Injectable()
export class SearchService {

  constructor(
    private http: HttpService,
    private sessionSearchService: SessionSearchService) { }

  public searchByNameAdmin(userName: IByNameRequest<string | null> ): Observable<IGeneralResponse<Array<ISearchAdminUserResponse>>> {
    return this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchAdminUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameadmin`, userName)
  }

  public onUserNameChange(userName: string): Observable<Array<ISearchAdminUserResponse> | undefined> {
    const adminKey: string = `adminsearch${userName}`
    const cachedUsers: Array<ISearchAdminUserResponse> | null = this.sessionSearchService.getData<Array<ISearchAdminUserResponse>>(adminKey)
    
    if (cachedUsers) {
      return of(cachedUsers)
    }
    else {
      const request: IByNameRequest<string | null> = {
        name: userName
      }
      return this.searchByNameAdmin(request)
        .pipe(
          map((resp) => {
            const users = resp.data
            
            if (users) {
              this.sessionSearchService.saveData(adminKey, users)
            }

            return users
          })
        );
    }
  }
}
