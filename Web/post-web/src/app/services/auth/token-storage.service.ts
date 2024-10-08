import { Injectable } from '@angular/core';
import { HttpService } from '../http.service';
import { identityServerUrl } from '../../urls';
import { IUser } from '../../models/User';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { IGeneralResponse } from '../../models/reponses/GeneralResponse';
import { take } from 'rxjs/internal/operators/take';

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

  public signOut(): void {
    this.deleteLocalStorageData()
    this.http.post<null, IGeneralResponse<null>>(`${identityServerUrl}/account/logout`, null)
      .pipe(take(1))
      .subscribe((resp) => console.log(resp.message));
  }

  public saveId(id: string): void {
    this.saveData(ID_KEY, id);
  }
  
  public saveUsername(username: string): void {
    this.saveData(USERNAME_KEY, username);
  }

  public saveRole(role: string): void {
    this.saveData(AUTHORITIES_KEY, role);
  }

  public getRole(): string | null {
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

  private saveData(key: string, data: string) {
    localStorage.removeItem(key)
    localStorage.setItem(key, data)
    this.updateUser()
  }

  private updateUser() {
    this.userSub.next({
      id: this.getId(),
      name: this.getUsername(),
      role: this.getRole()
    });
  }
}
