import { HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ResponseErrorHandlerService {

  constructor(private router: Router) { }

  public errorHandlers: { [key: number]: () => void } = {
    [HttpStatusCode.BadRequest]: () => this.Handle400(),
    [HttpStatusCode.InternalServerError]: () => this.Handle500(),
    [HttpStatusCode.Unauthorized]: () => this.Handle401(),
  };

  public Handle401(message: string = "Unauthorized") {
    console.error(JSON.stringify(message))
    this.router.navigate([`auth/login`])
  }

  public Handle400(message: string = "Bad request") {
    console.error(message)
  }

  public Handle500(message: string = "Internal server error") {
    console.error(JSON.stringify(message))
    this.router.navigate(['error'], { state: { error: '500' } })
  }

  public HandleDefault(message: any) {
    console.error(JSON.stringify(message));
  }
}
