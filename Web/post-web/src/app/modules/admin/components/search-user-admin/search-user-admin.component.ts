import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { ISearchAdminUserResponse } from '../../../../models/reponses/SearchAdminUserResponse';
import { FormControl } from '@angular/forms';
import { switchMap } from 'rxjs/internal/operators/switchMap';
import { distinctUntilChanged } from 'rxjs/internal/operators/distinctUntilChanged';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { startWith } from 'rxjs/internal/operators/startWith';
import { EMPTY } from 'rxjs/internal/observable/empty';
import { IByNameRequest } from '../../../../models/requests/user/ByNameRequest';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { identityServerUrl } from '../../../../urls';
import { map } from 'rxjs/internal/operators/map';
import { HttpService } from '../../../../services/http.service';
import { of } from 'rxjs/internal/observable/of';
import { SessionSearchService } from '../../../../services/session/session-search.service';
import { SearchService as SearchAdminService } from '../../services/search-admin.service';

@Component({
  selector: 'app-search-user-admin',
  templateUrl: './search-user-admin.component.html',
  styleUrl: './search-user-admin.component.scss'
})
export class SearchUserAdminComponent implements OnInit {
  protected userCtrl = new FormControl('')
  protected filteredUsers?: Observable<Array<ISearchAdminUserResponse>>

  constructor(
    private http: SearchAdminService,
    private sessionSearchService: SessionSearchService) { }

  ngOnInit(): void {
    this.filteredUsers = this.userCtrl.valueChanges.pipe(
      startWith(""),
      debounceTime(450),
      switchMap(value => {
        if (value === null || value.trim() === '') {
          return EMPTY;
        }
        return this.onUserNameChange(value.trim());
      }));
  }

  public fetchUserNameData(): string | null {
    return this.userCtrl.value
  }    

  private onUserNameChange(userName: string): Observable<Array<ISearchAdminUserResponse>> {
    const cachedUsers: Array<ISearchAdminUserResponse> | null = this.sessionSearchService.getData<Array<ISearchAdminUserResponse>>(`adminsearch${userName}`)
    if (cachedUsers) {
      return of(cachedUsers)
    }
    else {
      const request: IByNameRequest<string | null> = {
        name: userName
      }
      return this.http.searchByNameAdmin(request);
    }
  }
}
