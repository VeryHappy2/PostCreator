import { Component } from '@angular/core';
import { IUser } from '../../../models/User';
import { TokenStorageService } from '../../../services/auth/token-storage.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss'
})
export class NavigationComponent {
  public user?: IUser

  constructor(private token: TokenStorageService) { }

  ngOnInit(): void {
    if (this.token.getToken()) {
      this.user = {
        name: this.token.getUsername(),
        token: this.token.getToken(),
        role: this.token.getAuthorities(),
      }
    }
  }
}
