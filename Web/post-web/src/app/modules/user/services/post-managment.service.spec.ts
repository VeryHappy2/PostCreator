import { TestBed } from '@angular/core/testing';

import { PostManagmentService } from './post-managment.service';
import { HttpService } from '../../../services/http.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { of } from 'rxjs/internal/observable/of';
import { postUrl } from '../../../urls';

describe('PostManagmentService', () => {
  let service: PostManagmentService;
  let http: HttpService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        PostManagmentService,
        HttpService
      ]
    });
    http = TestBed.inject(HttpService)
    service = TestBed.inject(PostManagmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("should delete the post", () => {
    const req: IByIdRequest<number> = {
      id: 1
    }
    const resp: IGeneralResponse<number> = {
      flag: true,
      data: 1,
      message: "test"
    }

    spyOn(http, "post").and.returnValue(of(resp));

    service.deletePost(req).subscribe(data => {
      expect(data).toEqual(resp)
    })

    expect(http.post).toHaveBeenCalledOnceWith(`${postUrl}/postitem/delete`, req)
  })
});
