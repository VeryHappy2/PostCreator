import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpService } from './http.service';

describe('HttpService', () => {
  let service: HttpService;
  let httpTestingController: HttpTestingController

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
    });
    service = TestBed.inject(HttpService);
    httpTestingController = TestBed.inject(HttpTestingController)
  });

  afterEach(() => {
    httpTestingController.verify()
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("should to create the get request", () => {
    const response = {
      data: "data"
    }

    service.get("test").subscribe(resp => {
      expect(resp).toBe(response)
    })
    const req = httpTestingController.expectOne("test")

    req.flush(response)
    expect(req.request.method).toBe("GET")
    expect(req.request.url).toEqual("test")
  });

  it("should to create the post request", () => {
    const response = {
      data: "data"
    };

    service.post("test", { data: "data" }).subscribe(resp => {
      expect(resp).toBe(response);
    })
    const req = httpTestingController.expectOne("test");

    req.flush(response);
    expect(req.request.method).toBe("POST");
    expect(req.request.url).toEqual("test");
  });
});
