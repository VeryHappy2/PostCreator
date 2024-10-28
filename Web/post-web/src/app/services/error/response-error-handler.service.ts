import { HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { TokenStorageService } from '../auth/token-storage.service';
import { ErrorService } from './error.service';
import { IGeneralResponse } from '../../models/reponses/GeneralResponse';
import { identityServerUrl } from '../../../env/urls';
import { HttpService } from '../http.service';


@Injectable({
  providedIn: 'root'
})
export class ResponseErrorHandlerService {

  constructor(
    private router: Router,
    private tokenStorage: TokenStorageService,
    private http: HttpService,
    private errorService: ErrorService) { }

  public errorHandlers: { [key: number]: () => void } = {
    [HttpStatusCode.BadRequest]: () => this.Handle400(),
    [HttpStatusCode.InternalServerError]: () => this.Handle500(),
    [HttpStatusCode.Unauthorized]: () => this.Handle401(),
  }

  public Handle401(message: string = "Unauthorized"): void {
    console.error(message)

    this.http.get<IGeneralResponse<null>>(`${identityServerUrl}/account/refresh`)
      .pipe(take(1))
      .subscribe({
        next: (response) => {
          console.log(response.message)
        },
        error: () => {
          this.tokenStorage.deleteLocalStorageData()
          this.router.navigate([`auth/login`])
        }
      })
  }

  public Handle400(): void {
    console.error(`Bad request`)
  }

  public Handle500(message: string = "Internal server error"): void {
    console.error(JSON.stringify(message))
    this.errorService.saveError(message + " 500")
    this.router.navigate(['error'])
  }

  public HandleDefault(message: any): void {
    console.error(JSON.stringify(message));
  }

  public GetMessageError(response: any): string | undefined {
    if (response.errors) {
      const message = Object.values(response.errors).flat()
      return message.join(',');
    }
    
    return response.message
  } 
}
