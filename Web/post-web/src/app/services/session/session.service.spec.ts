import { TestBed } from '@angular/core/testing';
import { SessionService } from './session.service';

describe('SessionService', () => {
  let service: SessionService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SessionService
      ]
    });
    service = TestBed.inject(SessionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("get data from the sessionStorage", () => {
    const data: string = JSON.stringify(400);

    spyOn(sessionStorage, "getItem").and.returnValue(data);
    const result: number | null = service.getData<number>("item");

    expect(result).toEqual(400);
    expect(sessionStorage.getItem).toHaveBeenCalledWith("item");
  });

  it('should save data to sessionStorage', () => {
    const key = 'testKey';
    const data = "data";
    
    spyOn(sessionStorage, 'setItem');
  
    service.saveData(key, data);
  
    expect(sessionStorage.setItem).toHaveBeenCalledWith(key, JSON.stringify(data));
  });

  it('should remove data from the sessionStorage', () => {
    const key = 'testKey';
    
    spyOn(sessionStorage, 'removeItem');
  
    service.removeData(key);
  
    expect(sessionStorage.removeItem).toHaveBeenCalledWith(key);
  });
});
