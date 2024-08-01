import { Injectable } from '@angular/core';
import { SessionService } from './session.service';
import { ICache } from '../../models/cache';

@Injectable({
  providedIn: 'root'
})
export class SessionSearchService implements SessionService {

  constructor(private sessionsService: SessionService) { }

  public removeData(key: string): void {
    sessionStorage.removeItem(key);
  }

  public clearData(): void {
    sessionStorage.clear();
  }

  public saveData<TData>(key: string, data: TData) {
    const cacheData: ICache<TData> = {
      timestamp: new Date().getTime(),
      data: data
    };
    sessionStorage.setItem(key, JSON.stringify(cacheData))
  }

  public getData<TData>(key: string): TData | null {
    const cached = this.sessionsService.getData<ICache<TData>>(key);
    if (!cached) {
      return null;
    }

    const cacheDuration = 60000; // 60000 is 10 min
    if (new Date().getTime() - cached.timestamp > cacheDuration) {
      sessionStorage.removeItem(key);
      return null;
    }
    return cached.data
  }
}
