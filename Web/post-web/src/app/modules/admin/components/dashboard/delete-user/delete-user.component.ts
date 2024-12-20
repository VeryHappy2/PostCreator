import { Component, ViewChild } from '@angular/core';
import { IGeneralResponse } from '../../../../../models/responses/GeneralResponse';
import { IByNameRequest } from '../../../../../models/requests/user/ByNameRequest';
import { SearchUserAdminComponent } from '../../search-user-admin/search-user-admin.component';
import { take } from 'rxjs';
import { ModificationUserService } from '../../../services/modification-user.service';
import { ResponseErrorHandlerService } from '../../../../../services/error/response-error-handler.service';


@Component({
  selector: 'app-delete-user',
  templateUrl: './delete-user.component.html',
  styleUrl: './delete-user.component.scss'
})
export class DeleteUserComponent {
  @ViewChild("search") searcherUserAdmin!: SearchUserAdminComponent
  protected check?: IGeneralResponse<null>

  constructor(
    private modificationSerivice: ModificationUserService,
    private errorHandler: ResponseErrorHandlerService) { }

  protected delete() {
    const userName = this.searcherUserAdmin.fetchUserNameData();
    if (userName) {
      const request: IByNameRequest<string> = {
        name: this.searcherUserAdmin.fetchUserNameData()!
      };

      this.modificationSerivice.deleteUser(request)
        .pipe(take(1))
        .subscribe({
          next: (resp) => {
            this.check = resp
          },
          error: (err) => {
            this.check = {
              flag: false,
              message: this.errorHandler.GetMessageError(err.error),
              data: null
            }
          }
        });
    }
    else {
      console.warn("User name data is invalid");
      this.check = {
        flag: false,
        message: "User name data is emtpy",
      }
    }
  }
}
