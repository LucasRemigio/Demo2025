import { TestBed } from '@angular/core/testing';

import { AccountsManagmentService } from './logs.service';

describe('AccountsManagmentService', () => {
  let service: AccountsManagmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccountsManagmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
