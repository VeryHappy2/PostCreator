import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../../services/token-storage.service';
import { User } from '../../../../models/User';

@Component({
  selector: 'app-main',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public user?: User
  constructor(private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
    debugger
    this.user = {
      name: this.tokenStorage.getUsername(),
      role: this.tokenStorage.getAuthorities(),
      id: null,
      email: null
    }
  }
}


