import { Injectable } from '@angular/core';
import { HttpService } from '../http.service';
import { identityServerUrl } from '../../urls';
import { IUser } from '../../models/User';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

const ID_KEY = "authid"
const USERNAME_KEY = "authusername";
const AUTHORITIES_KEY = "authauthorities";

@Injectable({
  providedIn: 'root'
})

export class TokenStorageService {
  private userSub = new BehaviorSubject<IUser>({
    id: this.getId(),
    name: this.getUsername(),
    role: this.getRole()
  });

  public user$ = this.userSub.asObservable();
  
  constructor(private http: HttpService) { }

  public signOut() {
    this.deleteLocalStorageData()
    this.http.post(`${identityServerUrl}/account/logout`, null).subscribe((resp) => console.log(resp));
  }

  public saveId(id: string) {
    localStorage.removeItem(ID_KEY);
    localStorage.setItem(ID_KEY, id);
    this.updateUser();
  }
  
  public saveUsername(username: string): void {
    localStorage.removeItem(USERNAME_KEY);
    localStorage.setItem(USERNAME_KEY, username);
    this.updateUser();
  }

  public saveRole(role: string): void {
    localStorage.removeItem(AUTHORITIES_KEY)
    localStorage.setItem(AUTHORITIES_KEY, role)
    this.updateUser();
  }

  public getRole() {
    return localStorage.getItem(AUTHORITIES_KEY)
  }

  public getId() {
    return localStorage.getItem(ID_KEY)
  }
  
  public getUsername(): string | null {
    return localStorage.getItem(USERNAME_KEY);
  }

  public deleteLocalStorageData() {
    localStorage.clear();
    this.updateUser();
  }

  private updateUser() {
    this.userSub.next({
      id: this.getId(),
      name: this.getUsername(),
      role: this.getRole()
    });
  }
}
