import { TestBed } from '@angular/core/testing';

import { EmailActionsService } from './email-actions.service';

describe('EmailActionsService', () => {
  let service: EmailActionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmailActionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
