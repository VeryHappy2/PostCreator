import { HttpClient, HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TokenStorageService } from './auth/token-storage.service';
import { identityServerUrl } from '../urls';
import { IGeneralResponse } from '../models/reponses/GeneralResponse';
import { take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResponseErrorHandlerService {

  constructor(
    private router: Router,
    private tokenStorage: TokenStorageService,
    private http: HttpClient) { }

  public errorHandlers: { [key: number]: () => void } = {
    [HttpStatusCode.BadRequest]: () => this.Handle400(),
    [HttpStatusCode.InternalServerError]: () => this.Handle500(),
    [HttpStatusCode.Unauthorized]: () => this.Handle401(),
  };

  public Handle401(message: string = "Unauthorized"): void {
    console.error(message)

    this.http.get<IGeneralResponse<null>>(`${identityServerUrl}/account/refresh`, { withCredentials: true })
      .pipe(take(1))
      .subscribe({
        next: (response) => {
          console.log(response.message)
        },
        error: (err: HttpErrorResponse) => {
          this.tokenStorage.deleteLocalStorageData()
          this.router.navigate([`auth/login`])
        }
      })
  }

  public Handle400(message: string = "Bad request"): void {
    console.error(message)
  }

  public Handle500(message: string = "Internal server error"): void {
    console.error(JSON.stringify(message))
    this.router.navigate(['error'], { state: { error: '500' } })
  }

  public HandleDefault(message: any): void {
    console.error(JSON.stringify(message));
  }
}
