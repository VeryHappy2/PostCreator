import { TestBed } from '@angular/core/testing';

import { ModificationUserService } from './modification-user.service';

describe('ModificationUserService', () => {
  let service: ModificationUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModificationUserService);
  });

  xit('should be created', () => {
    expect(service).toBeTruthy();
  });
});
