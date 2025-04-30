import { TestBed } from '@angular/core/testing';

import { AddressUpdateService } from './address-update.service';

describe('AddressUpdateService', () => {
  let service: AddressUpdateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddressUpdateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
