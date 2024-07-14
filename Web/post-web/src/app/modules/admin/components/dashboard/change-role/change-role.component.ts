import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl } from '../../../../../urls';
import { map, Observable } from 'rxjs';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { MatSelectChange } from '@angular/material/select';
import { ChangeRoleRequest } from '../../../../../models/requests/user/ChangeRoleRequest';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { IUserResponse } from '../../../../../models/reponses/UserResponse';
import { IByIdRequest } from '../../../../../models/requests/ByIdRequest';
import { ByNameRequest } from '../../../../../models/requests/user/ByNameRequest';

@Component({
  selector: 'app-change-role',
  templateUrl: './change-role.component.html',
  styleUrl: './change-role.component.scss'
})
export class ChangeRoleComponent implements OnInit {
  roles$?: Observable<IGeneralResponse<Array<string>>>
  changeRoleRequest?: ChangeRoleRequest
  selectedRole?: string
  check?: IGeneralResponse<null>
  userCtrl = new FormControl('')
  filteredUsers?: Observable<Array<IUserResponse>>

  constructor(private http: HttpService) { }

  ngOnInit(): void {
    this.roles$ = this.http.get(`${identityServerUrl}/account/getroles`)
    this.userCtrl.valueChanges.subscribe(value => {
      this.onUserNameChange(value);
    });
  }

  public ChangeRole() {
    if (this.selectedRole && this.userCtrl.value) {
      let request: ChangeRoleRequest = {
        role: this.selectedRole,
        userName: this.userCtrl.value
      }
      
      this.http.post<ChangeRoleRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/changerole`, request)
      .subscribe((value: IGeneralResponse<null>) => this.check = value,
      (error: HttpErrorResponse) => this.check = error.error)
    }
  }

  public onSelectChange(event: MatSelectChange) {
    this.selectedRole = event.value
  }

  private onUserNameChange(userName: string | null) {
    const request: ByNameRequest<string | null> = {name: userName}
    this.filteredUsers = this.http.post<ByNameRequest<string | null>, IGeneralResponse<Array<IUserResponse>>>(`${identityServerUrl}/accountbff/searchbyname`, request).pipe(
      map(response => response.data || [])
    );
  }
}
