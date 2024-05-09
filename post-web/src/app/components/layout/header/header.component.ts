import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { TokenStorageService } from '../../../services/token-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  public userName: string | null | undefined
  public token?: string | null
  public roles?: string[]

  constructor(private _token: TokenStorageService) {}

  ngOnInit(): void {
    this.userName = this._token.getUsername()
    this.token = this._token.getToken()
  }
} 
