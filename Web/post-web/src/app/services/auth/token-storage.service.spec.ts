import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TokenStorageService } from './token-storage.service';
import { HttpService } from '../http.service';
import { identityServerUrl } from '../../urls';

const ID_KEY = "authid"
const USERNAME_KEY = "authusername";
const AUTHORITIES_KEY = "authauthorities";

describe('TokenStorageService', () => {
  let service: TokenStorageService;
  let httpTestingController: HttpTestingController;

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

    service = TestBed.inject(TokenStorageService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  afterEach(() => {
    httpTestingController.verify();
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

    service.signOut();

    const req = httpTestingController.expectOne(`${identityServerUrl}/account/logout`);

    expect(req.request.method).toBe("POST")
    req.flush({ flag: true, message: "Exited" })
    expect(console.log).toHaveBeenCalledWith("Exited")
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
  
});
