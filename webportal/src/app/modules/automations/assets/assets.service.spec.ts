import { TestBed } from '@angular/core/testing';

import { AssetsService } from './assets.service';

describe('AccountsManagmentService', () => {
  let service: AssetsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssetsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
