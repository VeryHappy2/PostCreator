import { Component, ViewChild } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl, postUrl } from '../../../../../urls';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { IByNameRequest } from '../../../../../models/requests/user/ByNameRequest';
import { take } from 'rxjs/internal/operators/take';
import { SearchUserAdminComponent } from '../../search-user-admin/search-user-admin.component';


@Component({
  selector: 'app-delete-user',
  templateUrl: './delete-user.component.html',
  styleUrl: './delete-user.component.scss'
})
export class DeleteUserComponent {
  @ViewChild("search") searcherUserAdmin!: SearchUserAdminComponent

  constructor(private http: HttpService) { }

  public delete(): void {
    if (this.searcherUserAdmin.fetchUserNameData()) {
      const request: IByNameRequest<string> = {
        name: this.searcherUserAdmin.fetchUserNameData()!
      }
      this.http.post<IByNameRequest<string>, IGeneralResponse<null>>(`${identityServerUrl}/account/delete`, request)
        .pipe(take(1))
        .subscribe((response) => console.log(response.message))
    }
  }
}
