import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { postUrl } from '../../../../env/urls';
import { IGeneralResponse } from '../../../models/responses/GeneralResponse';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';
import { Observable } from 'rxjs';

@Injectable()
export class PostManagmentService {

  constructor(private http: HttpService) { }

  public deletePost(request: IByIdRequest<number>): Observable<IGeneralResponse<number>> {
    return this.http.post<IByIdRequest<number>, IGeneralResponse<number>>(`${postUrl}/postitem/delete`, request)
  }
}
