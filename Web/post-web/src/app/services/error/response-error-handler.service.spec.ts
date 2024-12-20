import { TestBed } from '@angular/core/testing';
import { ResponseErrorHandlerService } from './response-error-handler.service';
import { ErrorService } from './error.service';
import { HttpService } from '../http.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { provideRouter, Router } from '@angular/router';
import { IGeneralResponse } from '../../models/responses/GeneralResponse';
import { of } from 'rxjs/internal/observable/of';
import { throwError } from 'rxjs';
import { TokenStorageService } from '../auth/token-storage.service';
import { Component } from '@angular/core';

describe('Response error handler', () => {
  let service: ResponseErrorHandlerService;
  let errorSerice: ErrorService
  let router: Router;
  let httpService: HttpService;
  let tokenStorage: TokenStorageService

  @Component({
    selector: "fake",
    template: ""
  })
  class FakeError { }

  @Component({
    selector: "fake-login",
    template: ""
  })
  class FakeAuthLogin { }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        ResponseErrorHandlerService,
        ErrorService,
        HttpService,
        Router,
        TokenStorageService,
        provideRouter([
          { path: "error", component: FakeError },
          { path: "auth/login", component: FakeAuthLogin }
        ])
      ]
    });

    tokenStorage = TestBed.inject(TokenStorageService)
    httpService = TestBed.inject(HttpService)
    router = TestBed.inject(Router)
    errorSerice = TestBed.inject(ErrorService)
    service = TestBed.inject(ResponseErrorHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("the 500 error", () => {
    const error: string = "The error is:";
    
    spyOn(console, "error").and.callThrough();
    spyOn(errorSerice, "saveError").and.callThrough();
    spyOn(router, "navigate").and.callThrough();
    
    service.Handle500(error);

    expect(console.error).toHaveBeenCalledOnceWith(JSON.stringify(error));
    expect(errorSerice.saveError).toHaveBeenCalledOnceWith(error + " 500");
    expect(router.navigate).toHaveBeenCalledOnceWith(["error"]);
  });

  it("the 400 error", () => {
    spyOn(console, "error").and.callThrough();
    
    service.Handle400()

    expect(console.error).toHaveBeenCalledOnceWith(`Bad request`)
  });

  it("the default handler ", () => {
    const message = "message";

    spyOn(console, "error").and.callThrough()

    service.HandleDefault(message)

    expect(console.error).toHaveBeenCalledOnceWith(JSON.stringify(message))
  });

  it("should return without errors, 401", () => {
    const message = "message";
    const response: IGeneralResponse<null> = {
      data: null,
      flag: true,
      message: "Exited"
    }

    spyOn(console, "error").and.callThrough();
    spyOn(httpService, "get").and.returnValue(of(response))
    spyOn(console, "log").and.callThrough()

    service.Handle401(message);

    expect(console.error).toHaveBeenCalledOnceWith(message);
    expect(console.log).toHaveBeenCalledOnceWith(response.message)
  });

  it("should return with error, 401", () => {
    const message = "message";

    spyOn(console, "error").and.callThrough();
    spyOn(httpService, "get").and.returnValue(throwError(() => new Error("Error")))
    spyOn(router, "navigate").and.callThrough()
    spyOn(tokenStorage, "deleteLocalStorageData").and.callThrough()

    service.Handle401(message);

    expect(console.error).toHaveBeenCalledOnceWith(message);
    expect(tokenStorage.deleteLocalStorageData).toHaveBeenCalled()
    expect(router.navigate).toHaveBeenCalledOnceWith([`auth/login`])
  });
});
