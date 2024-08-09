import { Component } from '@angular/core';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
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

  protected signOut(): void {
    this.tokenStorageService.signOut()
    this.route.navigate(['auth/login'])
  }
}
