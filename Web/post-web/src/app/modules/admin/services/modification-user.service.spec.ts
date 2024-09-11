import { TestBed } from '@angular/core/testing';

import { ModificationUserService } from './modification-user.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpService } from '../../../services/http.service';

describe('ModificationUserService', () => {
  let service: ModificationUserService;
  let http: HttpService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        HttpService,
        ModificationUserService
      ],
    });

    http = TestBed.inject(HttpService)
    service = TestBed.inject(ModificationUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
