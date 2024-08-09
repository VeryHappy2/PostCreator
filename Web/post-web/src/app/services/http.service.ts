import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})

export class HttpService {

  constructor(private httpClient: HttpClient) { }

  get<TResponse>(url: string): Observable<TResponse>{
    return this.httpClient.get<TResponse>(url, { withCredentials: true })
  }

  post<TRequest, TResponse>(url: string, data: TRequest): Observable<TResponse>{
    return this.httpClient.post<TResponse>(url, data, { withCredentials: true })
  }
}
