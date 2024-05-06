import { Injectable } from '@angular/core';
import { TokenStorageService } from './token-storage.service';
import { HttpHandler, HttpRequest } from '@angular/common/http';

const TOKEN_HEADER_KEY = 'Authorization';
@Injectable()

export class AuthInterceptorService {

  constructor(private token: TokenStorageService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
      let authReq = req;
      const token = this.token.getToken();
      if (token != null) {
          authReq = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, token) });
      }
      return next.handle(authReq);
    }
}
