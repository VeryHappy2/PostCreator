import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl } from '../../../../../urls';
import { map, Observable } from 'rxjs';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { MatSelectChange } from '@angular/material/select';
import { IChangeRoleRequest } from '../../../../../models/requests/user/ChangeRoleRequest';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { ISearchAdminUserResponse } from '../../../../../models/reponses/SearchAdminUserResponse';
import { IByNameRequest } from '../../../../../models/requests/user/ByNameRequest';

@Component({
  selector: 'app-change-role',
  templateUrl: './change-role.component.html',
  styleUrl: './change-role.component.scss'
})
export class ChangeRoleComponent implements OnInit {
  public roles$?: Observable<IGeneralResponse<Array<string>>>
  public changeRoleRequest?: IChangeRoleRequest
  public selectedRole?: string
  public check?: IGeneralResponse<null>
  public userCtrl = new FormControl('')
  public filteredUsers?: Observable<Array<ISearchAdminUserResponse>>

  constructor(private http: HttpService) { }

  ngOnInit(): void {
    this.roles$ = this.http.get(`${identityServerUrl}/account/getroles`)
    this.userCtrl.valueChanges.subscribe(value => {
      this.onUserNameChange(value);
    });
  }

  public changeRole() {
    if (this.selectedRole && this.userCtrl.value) {
      let request: IChangeRoleRequest = {
        role: this.selectedRole,
        userName: this.userCtrl.value
      }
      
      this.http.post<IChangeRoleRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/changerole`, request)
      .subscribe((value: IGeneralResponse<null>) => this.check = value,
      (error: HttpErrorResponse) => this.check = error.error)
    }
  }

  public onSelectChange(event: MatSelectChange) {
    this.selectedRole = event.value
  }

  private onUserNameChange(userName: string | null) {
    const request: IByNameRequest<string | null> = {
      name: userName
    }
    this.filteredUsers = this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchAdminUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameadmin`, request).pipe(
      map(response => response.data || [])
    );
  }
}
