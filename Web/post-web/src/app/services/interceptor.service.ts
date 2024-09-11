import { Injectable } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpHandler, HttpRequest } from '@angular/common/http';
import { catchError } from 'rxjs/internal/operators/catchError';
import { throwError } from 'rxjs/internal/observable/throwError';
import { ResponseErrorHandlerService } from './error/response-error-handler.service';

@Injectable({
  providedIn: 'root'
})

export class Interceptor {

  constructor(
    private errorResponse: ResponseErrorHandlerService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    let authReq = req;
    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        const handler = this.errorResponse.errorHandlers[error.status]
          || (() => this.errorResponse.HandleDefault(error));

        handler();
        return throwError(error);
      })
    );
  }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: Interceptor, multi: true }
];
