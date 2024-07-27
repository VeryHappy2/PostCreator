import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { identityServerUrl } from '../../../../urls';
import { Router } from '@angular/router';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { IUserRegisterRequest } from '../../../../models/requests/user/UserRegisterRequest';
import { HttpErrorResponse } from '@angular/common/http';
import { take } from 'rxjs/internal/operators/take';

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

  constructor(private http: HttpService, 
    private router: Router,
  ) { }

  public signUp(): void {
    const userRegister: IUserRegisterRequest = {
      name: this.userGroup.value.name!,
      email: this.userGroup.value.email!,
      password: this.userGroup.value.password!,
      confirmPassword: this.userGroup.value.confirmPassword!
    }

    if (userRegister.name && userRegister.email && userRegister.password && userRegister.confirmPassword) {
      this.http.post<IUserRegisterRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/register`, userRegister)
        .pipe(take(1))
        .subscribe(() => {
          this.router.navigate(['auth/login'])
        },
        (error: HttpErrorResponse) => {
          this.check = error.error
        });
    }
  }
}
