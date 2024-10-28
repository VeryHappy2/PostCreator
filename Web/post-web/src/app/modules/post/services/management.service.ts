import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { IPostCommentRequest } from '../../../models/requests/CommentRequest';
import { Observable } from 'rxjs';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { postUrl } from '../../../../env/urls';
import { IPostItemRequest } from '../../../models/requests/PostItemRequest';
import { IByIdRequest } from '../../../models/requests/ByIdRequest';

@Injectable()
export class ManagementService {

  constructor(private http: HttpService) { }

  public addComment(comment: IPostCommentRequest): Observable<IGeneralResponse<number>> {
    return this.http.post<IPostCommentRequest, IGeneralResponse<number>>(`${postUrl}/postcomment/add`, comment)
  }

  public addPost(post: IPostItemRequest): Observable<IGeneralResponse<number>> {
    return this.http.post<IPostItemRequest, IGeneralResponse<number>>(`${postUrl}/postitem/add`, post)
  }

  public addView(request: IByIdRequest<number>): Observable<IGeneralResponse<null>> {
    return this.http.post(`${postUrl}/postitem/addview`, request)
  }

  public addLike(request: IByIdRequest<number>): Observable<IGeneralResponse<null>> {
    return this.http.post(`${postUrl}/postlike/add`, request);
  }
}
