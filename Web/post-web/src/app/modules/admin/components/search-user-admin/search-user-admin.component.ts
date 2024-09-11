import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { ISearchAdminUserResponse } from '../../../../models/reponses/SearchAdminUserResponse';
import { FormControl } from '@angular/forms';
import { switchMap } from 'rxjs/internal/operators/switchMap';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { startWith } from 'rxjs/internal/operators/startWith';
import { EMPTY } from 'rxjs/internal/observable/empty';
import { SessionSearchService } from '../../../../services/session/session-search.service';
import { SearchService as SearchAdminService } from '../../services/search.service';

@Component({
  selector: 'app-search-user-admin',
  templateUrl: './search-user-admin.component.html',
  styleUrl: './search-user-admin.component.scss'
})
export class SearchUserAdminComponent implements OnInit {
  protected userCtrl = new FormControl('')
  protected filteredUsers?: Observable<Array<ISearchAdminUserResponse> | undefined>

  constructor(
    private search: SearchAdminService) { }

  ngOnInit(): void {
    this.filteredUsers = this.userCtrl.valueChanges.pipe(
      startWith(""),
      debounceTime(450),
      switchMap(value => {
        if (value === null || value.trim() === '') {
          return EMPTY;
        }

        return this.search.onUserNameChange(value.trim());
      }));
  }

  public fetchUserNameData(): string | null {
    return this.userCtrl.value
  }    
}
