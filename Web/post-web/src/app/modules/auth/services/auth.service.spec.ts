import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { provideRouter, Router } from '@angular/router';
import { HttpService } from '../../../services/http.service';
import { TokenStorageService } from '../../../services/auth/token-storage.service';
import { IUserLoginRequest } from '../../../models/requests/user/UserLoginRequest';
import { ILogInResponse } from '../../../models/reponses/LogInResponse';
import { identityServerUrl } from '../../../urls';
import { of } from 'rxjs/internal/observable/of';
import { Component } from '@angular/core';

describe('AuthService', () => {
  let service: AuthService;
  let tokenStorage: TokenStorageService;
  let router: Router;
  let http: HttpService;

  @Component({
    selector: "dashboard",
    template: ""
  })
  class FakeDashboard { }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        Router,
        AuthService,
        HttpService,
        TokenStorageService,
        provideRouter([
          { path: "user/dashboard", component: FakeDashboard}
        ])
      ]
    });

    http = TestBed.inject(HttpService)
    router = TestBed.inject(Router);
    service = TestBed.inject(AuthService);
    tokenStorage = TestBed.inject(TokenStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("should to login the user", () => {
    const request: IUserLoginRequest = {
      email: "myacc@gmail.com",
      password: "User@123"
    };
    const response: ILogInResponse = {
      flag: true,
      message: "Logged in",
      data: {
        userName: "name",
        role: "User",
        id: "id",
      }
    } 

    spyOn(tokenStorage, "saveId").and.callThrough();
    spyOn(tokenStorage, "saveRole").and.callThrough();
    spyOn(tokenStorage, "saveUsername").and.callThrough();
    spyOn(router, "navigate").and.callThrough()
    const spyHttp = spyOn(http, "post").and.returnValue(of(response))

    service.login(request).subscribe({
      next: (result) => {
        expect(result).toEqual(response);
        expect(tokenStorage.saveId).toHaveBeenCalledOnceWith(response.data?.id!);
        expect(tokenStorage.saveRole).toHaveBeenCalledOnceWith(response.data?.role!);
        expect(tokenStorage.saveUsername).toHaveBeenCalledOnceWith(response.data?.userName!);
        expect(router.navigate).toHaveBeenCalledOnceWith([`${response.data?.role.toLowerCase()}/dashboard`]);
      }
    })
    
    expect(spyHttp).toHaveBeenCalledOnceWith(`${identityServerUrl}/account/login`, request)
  })
});
