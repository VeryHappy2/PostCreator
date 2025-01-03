import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IGeneralResponse } from '../../../../models/responses/GeneralResponse';
import { IUserRegisterRequest } from '../../../../models/requests/user/UserRegisterRequest';
import { AuthService } from '../../services/auth.service';
import { take } from 'rxjs/internal/operators/take';
import { HttpErrorResponse } from '@angular/common/http';
import { ResponseErrorHandlerService } from '../../../../services/error/response-error-handler.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  public userGroup = new FormGroup({
    name: new FormControl ('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl ('', [Validators.required]),
    confirmPassword: new FormControl ('', [Validators.required])
  })
  public check?: IGeneralResponse<null>

  constructor(
    private authService: AuthService,
    private errorHandler: ResponseErrorHandlerService) { }

  public signUp(): void {
    const userRegister: IUserRegisterRequest = {
      name: this.userGroup.value.name!,
      email: this.userGroup.value.email!,
      password: this.userGroup.value.password!,
      confirmPassword: this.userGroup.value.confirmPassword!
    };

    if (userRegister.name && userRegister.email && userRegister.password && userRegister.confirmPassword) {
      this.authService.register(userRegister)
        .pipe(take(1))
        .subscribe({
          error: (err: HttpErrorResponse) => {
            this.check = {
              flag: false,
              message: this.errorHandler.GetMessageError(err.error),
              data: null
            }
          }
        });
    }
  }
}
