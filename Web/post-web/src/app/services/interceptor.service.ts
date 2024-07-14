import { Injectable } from '@angular/core';
import { TokenStorageService } from './auth/token-storage.service';
import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpHandler, HttpRequest } from '@angular/common/http';
import { ResponseErrorHandlerService } from './response-error-handler.service';
import { catchError } from 'rxjs/internal/operators/catchError';
import { throwError } from 'rxjs/internal/observable/throwError';

const TOKEN_HEADER_KEY = 'Authorization';

@Injectable({
  providedIn: 'root'
})

export class Interceptor {

  constructor(
    private token: TokenStorageService,
    private errorResponse: ResponseErrorHandlerService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
      let authReq = req;
      const token = this.token.getToken();
      if (token != null) {
        authReq = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, `Bearer ${token}`) });
      }
      return next.handle(authReq).pipe(
        catchError((error: HttpErrorResponse) => {
          const handler = this.errorResponse.errorHandlers[error.status] 
            || (() => this.errorResponse.HandleDefault(error)); // This a line set what is error code and then return method  
          handler();
          return throwError(error);
        })
      );
    }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: Interceptor, multi: true }
];
