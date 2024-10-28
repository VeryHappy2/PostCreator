import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor() { }

  public saveData<TData>(key: string, data: TData): void {
    sessionStorage.setItem(key, JSON.stringify(data))
  }

  public getData<TData>(key: string): TData | null {
    const data = sessionStorage.getItem(key)
    return data ? JSON.parse(data) : null
  }

  public removeData(key: string): void {
    return sessionStorage.removeItem(key)
  }

  public clearData(): void {
    sessionStorage.clear();
  }
}
