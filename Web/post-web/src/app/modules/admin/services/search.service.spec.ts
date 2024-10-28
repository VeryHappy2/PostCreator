import { TestBed } from '@angular/core/testing';

import { SearchService } from './search.service';
import { SessionSearchService } from '../../../services/session/session-search.service';
import { HttpService } from '../../../services/http.service';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';
import { ISearchAdminUserResponse } from '../../../models/reponses/SearchAdminUserResponse';
import { identityServerUrl } from '../../../../env/urls';
import { of } from 'rxjs/internal/observable/of';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('SearchService', () => {
  let service: SearchService;
  let http: HttpService;
  let search: SessionSearchService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        HttpService,
        SessionSearchService,
        SearchService
      ]
    });

    search = TestBed.inject(SessionSearchService)
    service = TestBed.inject(SearchService);
    http = TestBed.inject(HttpService);
  });
  
  it('should save the search result in session storage and return users', () => {
    const userNameRequest: IByNameRequest<string | null> = { name: 'JohnDoe' };
    const mockResponse: IGeneralResponse<ISearchAdminUserResponse[]> = {
      flag: true,
      message: "message",
      data: [
        { roleName: "user", userName: "johndoe2"  }
      ]
    };

    spyOn(http, "post").and.returnValue(of(mockResponse));
  
    service.searchByNameAdmin(userNameRequest).subscribe(users => {
      expect(users).toEqual(mockResponse);
    });
  
    expect(http.post).toHaveBeenCalledOnceWith(`${identityServerUrl}/accountbff/searchbynameadmin`, userNameRequest);
  });

  it('should save the user name to session', () => {
    const userNameRequest: string = 'JohnDoe';
    const mockResponse: Array<ISearchAdminUserResponse> | null = [
      { roleName: 'user', userName: 'johndoe2' }
    ];
  
    spyOn(search, 'getData').and.returnValue(mockResponse);
  
    const response = service.onUserNameChange(userNameRequest);
    
    response.subscribe(data => {
      expect(data).toEqual(mockResponse);
    });
  
    expect(search.getData).toHaveBeenCalledOnceWith(`adminsearch${userNameRequest}`);
  });
});
