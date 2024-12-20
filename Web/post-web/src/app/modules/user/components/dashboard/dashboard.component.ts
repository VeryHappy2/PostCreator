import { Component, OnDestroy, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { IUser } from '../../../../models/User';
import { HttpErrorResponse } from '@angular/common/http';
import { IPostItem } from '../../../../models/entities/PostItem';
import { IGeneralResponse } from '../../../../models/responses/GeneralResponse';
import { postUrl } from '../../../../../env/urls';
import { HttpService } from '../../../../services/http.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-main',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  protected user?: IUser
  protected posts?: IGeneralResponse<Array<IPostItem>>

  private userSub?: Subscription
  
  constructor(
    private tokenStorage: TokenStorageService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.userSub = this.tokenStorage.user$.subscribe((user) => this.user = user)

    this.userService.getPostsByOwnUserId().subscribe({
      next: (response: IGeneralResponse<Array<IPostItem>>) => this.posts = response,
      error: (error: HttpErrorResponse) => this.posts = error.error
    })
  }

  ngOnDestroy(): void {
    if (this.userSub) {
      this.userSub.unsubscribe()
    }
  }
}


