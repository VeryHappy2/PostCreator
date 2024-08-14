import { Component, OnDestroy, OnInit } from '@angular/core';
import { IUser } from '../../../models/User';
import { TokenStorageService } from '../../../services/auth/token-storage.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { HttpService } from '../../../services/http.service';
import { take } from 'rxjs/internal/operators/take';
import { IGeneralResponse } from '../../../models/reponses/GeneralResponse';
import { identityServerUrl } from '../../../urls';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnInit, OnDestroy {
  protected user?: IUser

  private userSub?: Subscription;

  constructor(
    private token: TokenStorageService,
    private http: HttpService,) { }

  ngOnInit(): void {
    this.userSub = this.token.user$.subscribe(user => {
      this.user = user;

      if (!this.user) {
        this.http.get<IGeneralResponse<null>>(`${identityServerUrl}/account/refresh`)
          .pipe(take(1))
          .subscribe({
            next: (response) => {
              console.log(response.message)
            },
            error: () => {
              this.token.deleteLocalStorageData()
            }
          })
      }
    });
  }
  
  ngOnDestroy(): void {
    if (this.userSub) {
      this.userSub.unsubscribe();
    }
  }
}
