import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { FormControl } from '@angular/forms';
import { switchMap } from 'rxjs/internal/operators/switchMap';
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
import { ISearchUserResponse } from '../../../../models/reponses/SearchUserResponse';

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrl: './search-user.component.scss'
})
export class SearchUserComponent implements OnInit {
  @Output() userName: EventEmitter<string> = new EventEmitter<string>()

  protected userCtrl = new FormControl('')
  protected filteredUsers?: Observable<Array<ISearchUserResponse>>

  constructor(
    private http: HttpService,
    private sessionService: SessionSearchService) { }

  ngOnInit(): void {
    this.filteredUsers = this.userCtrl.valueChanges.pipe(
      startWith(""),
      debounceTime(450),
      switchMap(value => {
        console.log("Search user name:" + value)
        this.userName.emit(value!);
        if (value === null || value.trim() === '') {
          return EMPTY;
        }
        return this.onUserNameChange(value.trim());
      }));
  }

  public fetchUserNameData(): string  | null {
      return this.userCtrl.value
  }    

  private onUserNameChange(userName: string): Observable<Array<ISearchUserResponse>> {
    const cachedUsers: Array<ISearchUserResponse> | null = this.sessionService.getData<Array<ISearchUserResponse>>(userName)

    if (cachedUsers) {
      return of(cachedUsers)
    }
    else {
      const request: IByNameRequest<string | null> = {
        name: userName
      }
      console.log(JSON.stringify(request))
      return this.http.post<IByNameRequest<string | null>, IGeneralResponse<Array<ISearchUserResponse>>>(`${identityServerUrl}/accountbff/searchbynameuser`, request).pipe(
        map(response => {
          const users = response.data || [];
          this.sessionService.saveData(userName, users);
          return users;
        }))
    }
  }
}
