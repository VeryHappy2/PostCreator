import { TestBed } from '@angular/core/testing';

import { ManagementService } from './management.service';
import { HttpService } from '../../../services/http.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ManagementService', () => {
  let service: ManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        ManagementService,
        HttpService
      ]
    });
    service = TestBed.inject(ManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
