import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../services/token-storage.service';
import { HttpService } from '../../../services/http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  public userName: string | null | undefined
  public token?: string | null
  public roles?: string[]

  constructor(
    private _token: TokenStorageService,
    private router: Router) {}

  ngOnInit(): void {
    this.userName = this._token.getUsername()
    this.token = this._token.getToken()
  }
} 
