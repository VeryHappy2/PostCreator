import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { identityServer } from '../../../../urls';
import { UserLoginRequest } from '../../../../general-models/requests/UserLoginRequest';
import { LogInResponse } from '../../../../general-models/reponses/LogInResponse';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from '../../services/jwt.service';
import { JwtClaims } from '../../../../general-models/reponses/JwtClaimsResponse';
import { TokenStorageService } from '../../../../services/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  public userGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl ('', [Validators.required]),
  })

  constructor(
    private http: HttpService,
    private route: ActivatedRoute,
    private router: Router,
    private jwt: JwtService,
    private tokenStorage: TokenStorageService) { }

  logIn(): void {
    const user: UserLoginRequest = {
      password: this.userGroup.value.password!,
      email: this.userGroup.value.email!
    };
    if (user.email && user.password) {
      console.log(user.password)
      this.http.post<UserLoginRequest, LogInResponse>(`${identityServer}/account/login`, user).subscribe((response: LogInResponse) =>{
        if(response.flag) {
          let claims: JwtClaims | null = this.jwt.decodeToken<JwtClaims>(response.token)

          if (claims) {
            this.tokenStorage.saveId(claims.nameid);
            this.tokenStorage.saveAuthorities(claims.roles);
            this.tokenStorage.saveUsername(claims.name);
            this.tokenStorage.saveToken(response.token)

            this.router.navigate([`user/dashboard`])
          }
        }
      })
    } 
  }
}
