import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GeneralResponse } from '../../../../general-models/reponses/GeneralResponse';
import { HttpService } from '../../../../services/http.service';
import { identityServer } from '../../../../urls';
import { UserRegisterRequest } from '../../../../general-models/requests/UserRegisterRequest';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

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
  public check$?: Observable<GeneralResponse<null>>

  constructor(private http: HttpService, 
    private router: Router,
  ) { }

  signUp(): void {
    const userRegister: UserRegisterRequest = {
      name: this.userGroup.value.name!,
      email: this.userGroup.value.email!,
      password: this.userGroup.value.password!,
      confirmPassword: this.userGroup.value.confirmPassword!
    }

    if (userRegister.name && userRegister.email && userRegister.password && userRegister.confirmPassword) {
      this.http.post<UserRegisterRequest, GeneralResponse<null>>(`${identityServer}/account/login`, userRegister)
        .subscribe((value: GeneralResponse<null>) => {
          if (value.flag) {
            this.router.navigate(['auth/login'])
          }
        });
    }
  }
}
