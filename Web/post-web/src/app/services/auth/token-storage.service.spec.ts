import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TokenStorageService } from './token-storage.service';
import { HttpService } from '../http.service';
import { identityServerUrl } from '../../../env/urls';
import { of } from 'rxjs/internal/observable/of';
import { IGeneralResponse } from '../../models/responses/GeneralResponse';
import { throwError } from 'rxjs';

const ID_KEY = "authid"
const USERNAME_KEY = "authusername";
const AUTHORITIES_KEY = "authauthorities";

describe('TokenStorageService', () => {
  let service: TokenStorageService;
  let http: HttpService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        TokenStorageService,
        HttpService
      ]
    });

    http = TestBed.inject(HttpService)
    service = TestBed.inject(TokenStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("should clear localstorage and update the user", () => {
    spyOn(localStorage, "clear");
    spyOn(service as any, "updateUser");

    service.deleteLocalStorageData();

    expect(localStorage.clear).toHaveBeenCalled();
    expect(service['updateUser']).toHaveBeenCalled();
  })

  it("should to sign out the user from the system", () => {
    spyOn(service, "deleteLocalStorageData").and.callThrough();
    spyOn(console, "log");
    spyOn(http, "post").and.returnValue(of({ message: "Exited" }));

    service.signOut();

    expect(http.post).toHaveBeenCalledOnceWith(`${identityServerUrl}/account/logout`, null)
    expect(console.log).toHaveBeenCalledWith("Exited")
    expect(service.deleteLocalStorageData).toHaveBeenCalled()
  })

  describe("To save an user", () => {
    it("should save an user name to the localstorage", () => {
      spyOn(service as any, "saveData")

      service.saveUsername("name")

      expect(service['saveData']).toHaveBeenCalledWith(USERNAME_KEY, "name")
      expect(service['saveData']).toHaveBeenCalled()
    })

    it("should save id to localstorage", () => {
      spyOn(service as any, "saveData")

      service.saveId("id")

      expect(service['saveData']).toHaveBeenCalledWith(ID_KEY, "id")
      expect(service['saveData']).toHaveBeenCalled()
    })

    it("should save a role to localstorage", () => {
      spyOn(service as any, "saveData")

      service.saveRole("role")

      expect(service['saveData']).toHaveBeenCalledWith(AUTHORITIES_KEY, "role")
      expect(service['saveData']).toHaveBeenCalled()
    })

    
  })
  
  describe("refreshAccessToken", () => {
    it("should to create a request to refresh a token (success)", () => {
      const resp: IGeneralResponse<null> = {
        flag: true,
        data: null,
        message: "success"
      }

      spyOn(http, "get").and.returnValue(of(resp))
      spyOn(console, "log").and.callThrough()

      service.refreshAccessToken()
      
      expect(http.get).toHaveBeenCalledOnceWith(`${identityServerUrl}/account/refresh`)
      expect(console.log).toHaveBeenCalledOnceWith(resp.message)
    })

    it("should to create a request to refresh a token (error)", () => {
      const resp: IGeneralResponse<null> = {
        flag: true,
        data: null,
        message: "failed"
      }

      spyOn(http, "get").and.returnValue(throwError(() => new Error("Error")))
      spyOn(service, "deleteLocalStorageData").and.callThrough()

      service.refreshAccessToken()
      
      expect(http.get).toHaveBeenCalledWith(`${identityServerUrl}/account/refresh`)
      expect(service.deleteLocalStorageData).toHaveBeenCalled()
    })
  })
});
