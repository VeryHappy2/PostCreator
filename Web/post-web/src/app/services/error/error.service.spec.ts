import { TestBed } from '@angular/core/testing';
import { ErrorService } from './error.service';

describe('ErrorService', () => {
  let service: ErrorService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ErrorService
      ]
    });
    service = TestBed.inject(ErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("the error should not be empty", () => {
    const error: string = "400"

    service.saveError(error)
    let result = service.error

    expect(result).toBe(error)
  })
});
