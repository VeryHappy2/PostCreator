import { Injectable } from '@angular/core';
import { TokenStorageService } from './token-storage.service';
import { Admin, User } from '../../roles';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private tokenStorage: TokenStorageService) { }

  public isLoggedIn(): boolean {
    const token: string | null = this.tokenStorage.getToken()
    const role: string | null = this.tokenStorage.getAuthorities()

    return !!(role && token);
  }

  public isAdmin(): boolean {
    const role: string | null = this.tokenStorage.getAuthorities()
    if (role === Admin) {
      return true
    } else {
      return false
    }
  }

  public isUser(): boolean {
    const role: string | null = this.tokenStorage.getAuthorities()
    if (role === User) {
      return true
    } else {
      return false
    }
  }
}
