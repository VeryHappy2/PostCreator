import { TestBed } from '@angular/core/testing';
import { SessionSearchService } from './session-search.service';
import { SessionService } from './session.service';
import { ICache } from '../../models/Сache';

describe('SessionSearchSerivce', () => {
  let service: SessionSearchService;
  let sessionService: jasmine.SpyObj<SessionService>;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SessionSearchService,
        { provide: SessionService, useValue: jasmine.createSpyObj('SessionService', ['getData'])}
      ]
    });
    service = TestBed.inject(SessionSearchService);
    sessionService = TestBed.inject(SessionService) as jasmine.SpyObj<SessionService>
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it("should get the data from localStorage", () => {
    const validTimeStamp = new Date().getTime() - 10000;
    const cached: ICache<string> = {
      timestamp: validTimeStamp,
      data: "data"
    };

    sessionService.getData.and.returnValue(cached);
    const result = service.getData("key");

    expect(result).toEqual(cached.data);
    expect(sessionService.getData).toHaveBeenCalledWith("key");
  });

  it("should get null because not found any data by key", () => {
    sessionService.getData.and.returnValue(null);
    const result = service.getData("key");

    expect(result).toBeNull()
    expect(sessionService.getData).toHaveBeenCalledWith("key")
  })

  it("should get null if cache is outdated", () => {
    const validTimeStamp = new Date().getTime() - 70000;
    const cached: ICache<string> = {
      timestamp: validTimeStamp,
      data: "data"
    };

    sessionService.getData.and.returnValue(cached);
    const result = service.getData("key");
    spyOn(sessionStorage, "removeItem")

    expect(result).toEqual(null);
    expect(sessionService.getData).toHaveBeenCalledWith("key");
  });

  it("should save the data", () => {
    const reqData = "data"
    const key = "key"

    const timestamp = 1000000

    jasmine.clock().install()
    jasmine.clock().mockDate(new Date(timestamp))
    const cacheData: ICache<string> = {
      timestamp: timestamp,
      data: reqData
    };
    spyOn(sessionStorage, "setItem")

    service.saveData(key, reqData)

    expect(sessionStorage.setItem).toHaveBeenCalledWith(key, JSON.stringify(cacheData))
    jasmine.clock().uninstall()
  })
});
