import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { identityServerUrl } from '../../../../urls';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from '../../services/jwt.service';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { Observable } from 'rxjs/internal/Observable';
import { IJwtClaims } from '../../../../models/JwtClaimsResponse';
import { ILogInResponse } from '../../../../models/reponses/LogInResponse';
import { IUserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  public userGroup = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    password: new FormControl ('', [Validators.required]),
  })
  public check?: ILogInResponse
  public hidePassword = true
  
  constructor(
    private http: HttpService,
    private router: Router,
    private jwt: JwtService,
    private tokenStorage: TokenStorageService) { }

  public logIn(): void {
    const user: IUserLoginRequest = {
      password: this.userGroup.value.password!,
      email: this.userGroup.value.userName!
    };

    if (user.email && user.password) {
      this.http.post<IUserLoginRequest, ILogInResponse>(`${identityServerUrl}/account/login`, user)
        .subscribe((response: ILogInResponse) => {
          let decodedToken: IJwtClaims | null = this.jwt.decodeToken<IJwtClaims>(response.token)

          if (decodedToken) {
            this.tokenStorage.saveId(decodedToken.id);
            this.tokenStorage.saveAuthorities(decodedToken.role);
            this.tokenStorage.saveUsername(decodedToken.name);
            this.tokenStorage.saveToken(response.token)

            this.router.navigate([`${decodedToken.role.toLowerCase()}/dashboard`])
          }
          else {
            this.check = response
          }
        },
        (error: HttpErrorResponse) => {
          this.check = error.error
        })
    } 
  }
}
