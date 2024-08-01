import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl } from '../../../../../urls';
import { Observable } from 'rxjs';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { MatSelectChange } from '@angular/material/select';
import { IChangeRoleRequest } from '../../../../../models/requests/user/ChangeRoleRequest';
import { HttpErrorResponse } from '@angular/common/http';
import { SearchUserAdminComponent } from '../../search-user-admin/search-user-admin.component';

@Component({
  selector: 'app-change-role',
  templateUrl: './change-role.component.html',
  styleUrl: './change-role.component.scss'
})
export class ChangeRoleComponent implements OnInit {
  @ViewChild("search") searcherUserAdmin!: SearchUserAdminComponent

  public roles$?: Observable<IGeneralResponse<Array<string>>>
  public changeRoleRequest?: IChangeRoleRequest
  public selectedRole?: string
  public check?: IGeneralResponse<null>

  constructor(
    private http: HttpService) { }

  ngOnInit(): void {
    this.roles$ = this.http.get(`${identityServerUrl}/account/getroles`)
  }

  public changeRole() {
    if (this.selectedRole && this.searcherUserAdmin.fetchUserNameData()) {
      let request: IChangeRoleRequest = {
        role: this.selectedRole,
        userName: this.searcherUserAdmin.fetchUserNameData()!
      }
      
      this.http.post<IChangeRoleRequest, IGeneralResponse<null>>(`${identityServerUrl}/account/changerole`, request)
      .subscribe((value: IGeneralResponse<null>) => this.check = value,
      (error: HttpErrorResponse) => this.check = error.error)
    }
  }

  public onSelectChange(event: MatSelectChange): void {
    this.selectedRole = event.value
  }
}
