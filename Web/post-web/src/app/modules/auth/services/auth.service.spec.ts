import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { HttpService } from '../../../services/http.service';
import { TokenStorageService } from '../../../services/auth/token-storage.service';
import { IUserLoginRequest } from '../../../models/requests/user/UserLoginRequest';
import { ILogInResponse } from '../../../models/reponses/LogInResponse';
import { identityServerUrl } from '../../../urls';

describe('AuthService', () => {
  let service: AuthService;
  let tokenStorage: TokenStorageService;
  let httpTestingController: HttpTestingController;
  let router: Router

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        Router,
        AuthService,
        HttpService,
        TokenStorageService
      ]
    });

    router = TestBed.inject(Router)
    httpTestingController = TestBed.inject(HttpTestingController)
    service = TestBed.inject(AuthService);
    tokenStorage = TestBed.inject(TokenStorageService)
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

    spyOn(tokenStorage, "saveId");
    spyOn(tokenStorage, "saveRole");
    spyOn(tokenStorage, "saveUsername");
    spyOn(router, "navigate")

    service.login(request).subscribe({
      next: (result) => {
        expect(result).toEqual(response);
        expect(tokenStorage.saveId).toHaveBeenCalledWith(response.data?.id!);
        expect(tokenStorage.saveRole).toHaveBeenCalledWith(response.data?.role!);
        expect(tokenStorage.saveUsername).toHaveBeenCalledWith(response.data?.userName!);
        expect(router.navigate).toHaveBeenCalledWith([`${response.data?.role.toLowerCase()}/dashboard`]);
      }
    })

    const req = httpTestingController.expectOne(`${identityServerUrl}/account/login`)

    expect(req.request.method).toBe("POST")
    req.flush(response)
    expect(req.request.url).toBe(`${identityServerUrl}/account/login`);
  })
});
