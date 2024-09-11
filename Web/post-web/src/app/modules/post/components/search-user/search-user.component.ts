import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { FormControl } from '@angular/forms';
import { switchMap } from 'rxjs/internal/operators/switchMap';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { startWith } from 'rxjs/internal/operators/startWith';
import { EMPTY } from 'rxjs/internal/observable/empty';
import { IByNameRequest } from '../../../../models/requests/user/ByNameRequest';
import { HttpService } from '../../../../services/http.service';
import { of } from 'rxjs/internal/observable/of';
import { SessionSearchService } from '../../../../services/session/session-search.service';
import { ISearchUserResponse } from '../../../../models/reponses/SearchUserResponse';
import { SearchService } from '../../services/search.service';

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
    private sessionService: SessionSearchService,
    private searchUser: SearchService) { }

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
      
      return this.searchUser.searchUsersByName(request)
    }
  }
}
