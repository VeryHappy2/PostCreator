import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ILogInResponse } from '../../../../models/reponses/LogInResponse';
import { IUserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';
import { AuthService } from '../../services/auth.service';
import { take } from 'rxjs';


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
  protected check?: ILogInResponse | void
  protected hidePassword = true
  
  constructor(
    private authService: AuthService) { }

  protected logIn(): void {
    const user: IUserLoginRequest = {
      password: this.userGroup.value.password!,
      email: this.userGroup.value.userName!
    };

    if (user.email && user.password) {
      this.authService
        .login(user)
        .pipe(take(1))
        .subscribe(result => {
          this.check = result
        });
    } 
  }
}
