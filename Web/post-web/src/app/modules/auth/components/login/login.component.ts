import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { identityServerUrl } from '../../../../urls';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from '../../services/jwt.service';
import { TokenStorageService } from '../../../../services/token-storage.service';
import { Observable } from 'rxjs/internal/Observable';
import { JwtClaims } from '../../../../models/JwtClaimsResponse';
import { LogInResponse } from '../../../../models/reponses/LogInResponse';
import { UserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';

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
  public check?: LogInResponse
  public hidePassword = true
  
  constructor(
    private http: HttpService,
    private router: Router,
    private jwt: JwtService,
    private tokenStorage: TokenStorageService) { }

  public logIn(): void {
    const user: UserLoginRequest = {
      password: this.userGroup.value.password!,
      email: this.userGroup.value.userName!
    };

    if (user.email && user.password) {
      this.http.post<UserLoginRequest, LogInResponse>(`${identityServerUrl}/account/login`, user)
        .subscribe((response: LogInResponse) => {
          let decodedToken: JwtClaims | null = this.jwt.decodeToken<JwtClaims>(response.token)

          if (decodedToken) {
            this.tokenStorage.saveId(decodedToken.id);
            this.tokenStorage.saveAuthorities(decodedToken.role);
            this.tokenStorage.saveUsername(decodedToken.name);
            this.tokenStorage.saveToken(response.token)

            this.router.navigate([`user/dashboard`])
          }
          else {
            this.check = response
          }
        },
        (error: any) => {
          console.log(JSON.stringify(error))
          this.check == error
        })
    } 
  }
}
