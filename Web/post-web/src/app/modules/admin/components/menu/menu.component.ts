import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})
export class MenuComponent implements OnInit {
  public role?: string | null

  constructor(
    private tokenStorageService: TokenStorageService,
    private route: Router) { }

  ngOnInit(): void {
    this.role = this.tokenStorageService.getAuthorities()
  }  

  public signOut() {
    this.tokenStorageService.signOut()
    this.route.navigate(['auth/login'])
  }
}
