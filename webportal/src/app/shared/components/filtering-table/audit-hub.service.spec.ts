import { TestBed } from '@angular/core/testing';

import { AuditHubService } from './audit-hub.service';

describe('AuditHubService', () => {
  let service: AuditHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuditHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
