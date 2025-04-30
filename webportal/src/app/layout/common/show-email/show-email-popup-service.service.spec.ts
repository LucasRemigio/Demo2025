import { TestBed } from '@angular/core/testing';

import { ShowEmailPopupServiceService } from './show-email-popup-service.service';

describe('ShowEmailPopupServiceService', () => {
  let service: ShowEmailPopupServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShowEmailPopupServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
