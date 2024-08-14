import { Component, ViewChild } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl, postUrl } from '../../../../../urls';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { IByNameRequest } from '../../../../../models/requests/user/ByNameRequest';
import { SearchUserAdminComponent } from '../../search-user-admin/search-user-admin.component';
import { take } from 'rxjs';
import { ModificationUserService } from '../../../services/modification-user.service';


@Component({
  selector: 'app-delete-user',
  templateUrl: './delete-user.component.html',
  styleUrl: './delete-user.component.scss'
})
export class DeleteUserComponent {
  @ViewChild("search") searcherUserAdmin!: SearchUserAdminComponent
  protected check?: IGeneralResponse<null>

  constructor(private modificationSerivice: ModificationUserService) { }

  protected async delete() {
    const userName = this.searcherUserAdmin.fetchUserNameData();
    if (userName) {
      const request: IByNameRequest<string> = {
        name: this.searcherUserAdmin.fetchUserNameData()!
      };

      this.check = await this.modificationSerivice.deleteUserAsync(request);
    }
    else {
      console.warn("User name data is invalid");
    }
  }
}
