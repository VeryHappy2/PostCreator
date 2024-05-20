import { Injectable } from '@angular/core';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private tokenStorage: TokenStorageService) { }

  isLoggedIn(): boolean {
    const token: string | null = this.tokenStorage.getToken()
    return !!token;
  }
}
