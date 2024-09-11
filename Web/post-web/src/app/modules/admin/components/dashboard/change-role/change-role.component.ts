import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl } from '../../../../../urls';
import { Observable, take } from 'rxjs';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { MatSelectChange } from '@angular/material/select';
import { IChangeRoleRequest } from '../../../../../models/requests/user/ChangeRoleRequest';
import { SearchUserAdminComponent } from '../../search-user-admin/search-user-admin.component';
import { ModificationUserService } from '../../../services/modification-user.service';

@Component({
  selector: 'app-change-role',
  templateUrl: './change-role.component.html',
  styleUrl: './change-role.component.scss'
})
export class ChangeRoleComponent implements OnInit {
  @ViewChild("search") searcherUserAdmin!: SearchUserAdminComponent

  public roles$?: Observable<IGeneralResponse<Array<string>>>
  protected changeRoleRequest?: IChangeRoleRequest
  protected selectedRole?: string
  protected check?: IGeneralResponse<null>

  constructor(
    private http: HttpService,
    private modificationService: ModificationUserService) { }

  ngOnInit(): void {
    this.roles$ = this.http.get(`${identityServerUrl}/account/getroles`)
  }

  public changeRole(): void {
    const userName = this.searcherUserAdmin.fetchUserNameData()
    if (this.selectedRole && userName) {
      let request: IChangeRoleRequest = {
        role: this.selectedRole,
        userName: userName!
      };
      
      this.modificationService.changeRole(request)
        .pipe(take(1))
        .subscribe({
          error: (response) => {
            this.check = response
          }
        });
    }
    else {
      this.check = {
        flag: false,
        data: null,
        message: "You should to select a role or an user name"
      }
    }
  }

  public onSelectChange(event: MatSelectChange): void {
    this.selectedRole = event.value
  }
}
