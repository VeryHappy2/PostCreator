import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { IUser } from '../../../../models/User';

@Component({
  selector: 'app-main',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public user?: IUser

  constructor(
    private tokenStorage: TokenStorageService,
  ) { }

  ngOnInit(): void {
    this.user = {
      name: this.tokenStorage.getUsername(),
      role: this.tokenStorage.getAuthorities(),
      token: null
    }
  }
}