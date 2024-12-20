import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ILogInResponse } from '../../../../models/responses/LogInResponse';
import { IUserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';
import { AuthService } from '../../services/auth.service';
import { take } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ResponseErrorHandlerService } from '../../../../services/error/response-error-handler.service';


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
  protected check?: ILogInResponse
  protected hidePassword = true
  
  constructor(
    private authService: AuthService,
    private errorHandler: ResponseErrorHandlerService) { }

  protected logIn(): void {
    const user: IUserLoginRequest = {
      password: this.userGroup.value.password!,
      email: this.userGroup.value.userName!
    };

    if (user.email && user.password) {
      this.authService
        .login(user)
        .pipe(take(1))
        .subscribe({ 
          error: (err: HttpErrorResponse) => {
            this.check = {
              flag: false,
              message: this.errorHandler.GetMessageError(err.error),
            }
          }
        });
    } 
  }
}
