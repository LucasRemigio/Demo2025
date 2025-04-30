import { TestBed } from '@angular/core/testing';

import { ScriptsService } from './scripts.service';

describe('AccountsManagmentService', () => {
  let service: ScriptsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScriptsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
