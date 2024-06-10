import { Component } from '@angular/core';
import { TokenStorageService } from '../../../../services/token-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})
export class MenuComponent {
  constructor(
    private tokenStorageService: TokenStorageService,
    private route: Router) { }

  public signOut() {
    this.tokenStorageService.signOut()
    this.route.navigate(['home'])
  }
}
