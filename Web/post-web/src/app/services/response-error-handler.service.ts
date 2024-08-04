import { HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TokenStorageService } from './auth/token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class ResponseErrorHandlerService {

  constructor(
    private router: Router,
    private tokenStorage: TokenStorageService) { }

  public errorHandlers: { [key: number]: () => void } = {
    [HttpStatusCode.BadRequest]: () => this.Handle400(),
    [HttpStatusCode.InternalServerError]: () => this.Handle500(),
    [HttpStatusCode.Unauthorized]: () => this.Handle401(),
  };

  public Handle401(message: string = "Unauthorized"): void {
    console.error(JSON.stringify(message))
    this.tokenStorage.deleteLocalStorageData()
    this.router.navigate([`auth/login`])
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
