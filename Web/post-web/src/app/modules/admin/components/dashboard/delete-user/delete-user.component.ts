import { Component } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { identityServerUrl, postUrl } from '../../../../../urls';
import { FormControl } from '@angular/forms';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { ISearchAdminUserResponse } from '../../../../../models/reponses/SearchAdminUserResponse';
import { IByNameRequest } from '../../../../../models/requests/user/ByNameRequest';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { take } from 'rxjs/internal/operators/take';


@Component({
  selector: 'app-delete-user',
  templateUrl: './delete-user.component.html',
  styleUrl: './delete-user.component.scss'
})
export class DeleteUserComponent {
  public userNameCtrl = new FormControl('')
  public filteredUsers?: Observable<Array<ISearchAdminUserResponse>>

  constructor(private http: HttpService) { }

  ngOnInit(): void {
    this.userNameCtrl.valueChanges.subscribe(value => {
      this.onUserNameChange(value);
    });
  }

  public delete(): void {
    if (this.userNameCtrl.value) {
      const request: IByNameRequest<string> = {
        name: this.userNameCtrl.value
      }
      this.http.post<IByNameRequest<string>, IGeneralResponse<null>>(`${identityServerUrl}/account/delete`, request)
        .pipe(take(1))
        .subscribe((response) => console.log(response.message))
    }
  }

  private onUserNameChange(userName: string | null): void {
    const request: IByNameRequest<string | null> = {
      name: userName
    }
    this.filteredUsers = this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchAdminUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameadmin`, request).pipe(
      map(response => response.data || [])
    );
  }
}
