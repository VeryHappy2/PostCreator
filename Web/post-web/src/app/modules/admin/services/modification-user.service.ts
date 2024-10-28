import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { IChangeRoleRequest } from '../../../models/requests/user/ChangeRoleRequest';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { take } from 'rxjs/internal/operators/take';
import { lastValueFrom, Observable } from 'rxjs';
import { identityServerUrl } from '../../../../env/urls';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';

@Injectable()
export class ModificationUserService {

  constructor(private http: HttpService) { }

  public changeRole(request: IChangeRoleRequest): Observable<IGeneralResponse<null>> {
    return this.http.post<IChangeRoleRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/changerole`, request)
  }

  public deleteUser(request: IByNameRequest<string>): Observable<IGeneralResponse<null>> {
    return this.http.post<IByNameRequest<string>, IGeneralResponse<null>>(`${identityServerUrl}/account/delete`, request)
  }
}
