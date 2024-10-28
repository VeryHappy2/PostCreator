import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { HttpService } from '../../../services/http.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { IPostItem } from '../../../models/enities/PostItem';
import { of } from 'rxjs';
import { postUrl } from '../../../../env/urls';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';

describe('UserService', () => {
  let service: UserService;
  let http: HttpService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        UserService,
        HttpService
      ]
    });

    http = TestBed.inject(HttpService);
    service = TestBed.inject(UserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("should get posts by own user Id", () => {
    const resp: IGeneralResponse<Array<IPostItem>> = {
      flag: true,
      message: "message",
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
      ]
    }

    spyOn(http, "get").and.returnValue(of(resp))

    service.getPostsByOwnUserId().subscribe((data) => {
      expect(data).toEqual(resp)
    });

    expect(http.get).toHaveBeenCalledOnceWith(`${postUrl}/postbff/getpostsbyownuserid`)
  });

  it("should get posts by user Id", () => {
    const resp: IGeneralResponse<Array<IPostItem>> = {
      flag: true,
      message: "message",
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
      ]
    }

    const request: IByIdRequest<string> = {
      id: "1"
    } 

    spyOn(http, "post").and.returnValue(of(resp))

    service.getPostsByUserId(request).subscribe((data) => {
      expect(data).toEqual(resp)
    });

    expect(http.post).toHaveBeenCalledOnceWith(`${postUrl}/postbff/getpostsbyuserid`, request)
  });
});
