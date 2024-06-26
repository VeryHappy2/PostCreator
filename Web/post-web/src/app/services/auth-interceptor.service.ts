import { Injectable } from '@angular/core';
import { TokenStorageService } from './token-storage.service';
import { HTTP_INTERCEPTORS, HttpHandler, HttpRequest } from '@angular/common/http';

const TOKEN_HEADER_KEY = 'Authorization';

@Injectable({
  providedIn: 'root'
})

export class AuthInterceptor {

  constructor(private token: TokenStorageService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
      let authReq = req;
      const token = this.token.getToken();
      if (token != null) {
        authReq = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, `Bearer ${token}`) });
      }
      return next.handle(authReq);
    }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
];
