import { Injectable } from '@angular/core';

const TOKEN_KEY = 'authtoken';
const ID_KEY = "authid"
const USERNAME_KEY = "authusername";
const AUTHORITIES_KEY = "authauthorities";

@Injectable({
  providedIn: 'root'
})

export class TokenStorageService {
  public signOut() {
    window.sessionStorage.clear();
  }

  public saveToken(token: string) {
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  public saveId(id: string) {
    window.sessionStorage.removeItem(ID_KEY);
    window.sessionStorage.setItem(ID_KEY, id);
  }
  
  public saveUsername(username: string): void {
    window.sessionStorage.removeItem(USERNAME_KEY);
    window.sessionStorage.setItem(USERNAME_KEY, username);
  }

  public saveAuthorities(authorities: string) {
    window.sessionStorage.removeItem(AUTHORITIES_KEY);
    window.sessionStorage.setItem(AUTHORITIES_KEY, authorities);
  }

  public getId() {
    return window.sessionStorage.getItem(ID_KEY)
  }
  
  public getUsername(): string | null {
    return sessionStorage.getItem(USERNAME_KEY);
  }

  public getToken(): string | null {
    return window.sessionStorage.getItem(TOKEN_KEY);
  }

  public getAuthorities(): string | null {
    return window.sessionStorage.getItem(AUTHORITIES_KEY)
  }
}
