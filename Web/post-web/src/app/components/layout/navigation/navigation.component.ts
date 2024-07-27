import { Component, OnDestroy, OnInit } from '@angular/core';
import { IUser } from '../../../models/User';
import { TokenStorageService } from '../../../services/auth/token-storage.service';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent implements OnInit, OnDestroy {
  public user?: IUser
  private userSub?: Subscription;

  constructor(private token: TokenStorageService) { }

  ngOnInit(): void {
    this.userSub = this.token.user$.subscribe(user => {
      this.user = user;
    });
  }
  
  ngOnDestroy(): void {
    if (this.userSub) {
      this.userSub.unsubscribe();
    }
  }
}
