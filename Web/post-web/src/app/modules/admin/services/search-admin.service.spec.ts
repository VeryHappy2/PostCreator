import { TestBed } from '@angular/core/testing';

import { SearchService } from './search-admin.service';
import { SessionSearchService } from '../../../services/session/session-search.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpService } from '../../../services/http.service';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';
import { ISearchAdminUserResponse } from '../../../models/reponses/SearchAdminUserResponse';
import { identityServerUrl } from '../../../urls';

describe('SearchService', () => {
  let service: SearchService;
  let httpMock: HttpTestingController;
  let sessionSearchService: SessionSearchService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        HttpService,
        SearchService,
        {
          provide: SessionSearchService,
          useValue: {
            saveData: jasmine.createSpy('saveData')
          }
        }
      ]
    });

    service = TestBed.inject(SearchService);
    httpMock = TestBed.inject(HttpTestingController);
    sessionSearchService = TestBed.inject(SessionSearchService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should save the search result in session storage and return users', () => {
    const userNameRequest: IByNameRequest<string | null> = { name: 'JohnDoe' };
    const mockResponse: ISearchAdminUserResponse[] = [
      { roleName: "user", userName: "johndoe2"  }
    ];

    service.searchByNameAdmin(userNameRequest).subscribe(users => {
      expect(users).toEqual(mockResponse);
      expect(sessionSearchService.saveData).toHaveBeenCalledWith(`adminsearch${userNameRequest.name}`, mockResponse);
    });

    const req = httpMock.expectOne(`${identityServerUrl}/accountbff/searchbynameadmin`);

    expect(req.request.method).toBe('POST');
    req.flush({ data: mockResponse })
  });
});
