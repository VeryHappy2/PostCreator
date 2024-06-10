import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { identityServerUrl } from '../../../../urls';
import { Router } from '@angular/router';
import { GeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { UserRegisterRequest } from '../../../../models/requests/user/UserRegisterRequest';

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
  public check?: GeneralResponse<null>

  constructor(private http: HttpService, 
    private router: Router,
  ) { }

  public signUp(): void {
    const userRegister: UserRegisterRequest = {
      name: this.userGroup.value.name!,
      email: this.userGroup.value.email!,
      password: this.userGroup.value.password!,
      confirmPassword: this.userGroup.value.confirmPassword!
    }

    if (userRegister.name && userRegister.email && userRegister.password && userRegister.confirmPassword) {
      this.http.post<UserRegisterRequest, GeneralResponse<null>>(`${identityServerUrl}/account/register`, userRegister)
        .subscribe((value: GeneralResponse<null>) => {
          if (value.flag) {
            this.router.navigate(['auth/login'])
          }
          else {
            this.check = value
          }
        },
        (error: any) => {
          console.log(error)
          this.check = error
        });
    }
  }
}
