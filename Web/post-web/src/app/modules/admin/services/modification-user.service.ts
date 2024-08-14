import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { IChangeRoleRequest } from '../../../models/requests/user/ChangeRoleRequest';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { HttpErrorResponse } from '@angular/common/http';
import { take } from 'rxjs/internal/operators/take';
import { catchError, lastValueFrom, tap } from 'rxjs';
import { identityServerUrl } from '../../../urls';
import { IByNameRequest } from '../../../models/requests/user/ByNameRequest';

@Injectable()
export class ModificationUserService {

  constructor(private http: HttpService) { }

  public async changeRoleAsync(request: IChangeRoleRequest): Promise<IGeneralResponse<null>> {
    try {
      return await lastValueFrom( 
          this.http.post<IChangeRoleRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/changerole`, request)
            .pipe(take(1))
        )
    } catch (error: any) {
      return error.error
    }
  }

  public async deleteUserAsync(request: IByNameRequest<string>): Promise<IGeneralResponse<null>> {
    try {
      return await lastValueFrom(
        this.http.post<IByNameRequest<string>, IGeneralResponse<null>>(`${identityServerUrl}/account/delete`, request)
          .pipe(take(1))
      )
    
    } catch(error: any) {
      return error.error
    }
  }
}
