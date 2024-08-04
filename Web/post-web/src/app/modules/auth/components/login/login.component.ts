import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { identityServerUrl } from '../../../../urls';
import { Router } from '@angular/router';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { ILogInResponse } from '../../../../models/reponses/LogInResponse';
import { IUserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';
import { HttpErrorResponse } from '@angular/common/http';
import { take } from 'rxjs/internal/operators/take';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  protected userGroup = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    password: new FormControl ('', [Validators.required]),
  })
  protected check?: ILogInResponse
  protected hidePassword = true
  
  constructor(
    private http: HttpService,
    private router: Router,
    private tokenStorage: TokenStorageService) { }

  protected logIn(): void {
    const user: IUserLoginRequest = {
      password: this.userGroup.value.password!,
      email: this.userGroup.value.userName!
    };

    if (user.email && user.password) {
      this.http.post<IUserLoginRequest, ILogInResponse>(`${identityServerUrl}/account/login`, user)
        .pipe(take(1))
        .subscribe((response: ILogInResponse) => {
            this.tokenStorage.saveId(response.data?.id!);
            this.tokenStorage.saveUsername(response.data?.userName!);
            this.tokenStorage.saveRole(response.data?.role!)

            this.router.navigate([`${response.data?.role.toLowerCase()}/dashboard`])
        },
        (error: HttpErrorResponse) => {
          this.check = error.error
        })
    } 
  }
}
