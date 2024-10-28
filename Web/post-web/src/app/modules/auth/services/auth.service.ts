import { Injectable } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { Router } from '@angular/router';
import { TokenStorageService } from '../../../services/auth/token-storage.service';
import { IUserLoginRequest } from '../../../models/requests/user/UserLoginRequest';
import { ILogInResponse } from '../../../models/reponses/LogInResponse';
import { identityServerUrl } from '../../../../env/urls';
import { take } from 'rxjs/internal/operators/take';
import { HttpErrorResponse } from '@angular/common/http';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { IUserRegisterRequest } from '../../../models/requests/user/UserRegisterRequest';
import { catchError, Observable, of, tap } from 'rxjs';

@Injectable()
export class AuthService {

  constructor(
    private http: HttpService,
    private router: Router,
    private tokenStorage: TokenStorageService) { }

  public login(user: IUserLoginRequest): Observable<ILogInResponse> {
    return this.http.post<IUserLoginRequest, ILogInResponse>(`${identityServerUrl}/account/login`, user)
      .pipe(
        take(1),
        tap((response: ILogInResponse) => {
          const data = response.data
          if (data?.id && 
            data.role && 
            data.userName) {
            this.tokenStorage.saveId(data.id);
            this.tokenStorage.saveUsername(data.userName);
            this.tokenStorage.saveRole(data.role);
            this.router.navigate([`${data.role.toLowerCase()}/dashboard`]);
          }
        })
      );
  }
  
  public register(userRegister: IUserRegisterRequest): Observable<IGeneralResponse<null>> {
    return this.http.post<IUserRegisterRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/register`, userRegister)
      .pipe(
        take(1),
        tap(() => this.router.navigate(['auth/login'])),
        catchError((error: HttpErrorResponse) => {
          return of(error.error)
        })
      );
  }
}
