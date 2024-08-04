import { Component, OnDestroy, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { IUser } from '../../../../models/User';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-main',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  protected user?: IUser

  private userSub?: Subscription
  
  constructor(
    private tokenStorage: TokenStorageService,
  ) { }

  ngOnInit(): void {
    this.userSub = this.tokenStorage.user$.subscribe((user) => this.user = user)
  }

  ngOnDestroy(): void {
    if (this.userSub) {
      this.userSub.unsubscribe()
    }
  }
}