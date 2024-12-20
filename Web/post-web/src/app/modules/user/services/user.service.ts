import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';
import { IGeneralResponse } from '../../../models/responses/GeneralResponse';
import { postUrl } from '../../../../env/urls';
import { Observable } from 'rxjs';
import { IPostItem } from '../../../models/entities/PostItem';

@Injectable()
export class UserService {

  constructor(private http: HttpService) { }

  public getPostsByOwnUserId(): Observable<IGeneralResponse<Array<IPostItem>>> {
    return this.http.get<IGeneralResponse<Array<IPostItem>>>(`${postUrl}/postbff/getpostsbyownuserid`)
  }

  public getPostsByUserId(request: IByIdRequest<string>): Observable<IGeneralResponse<Array<IPostItem>>> {
    return this.http.post<IByIdRequest<string>, IGeneralResponse<Array<IPostItem>>>(`${postUrl}/postbff/getpostsbyuserid`, request)
  }
}
