import { TestBed } from '@angular/core/testing';

import { SwitchViewService } from './switch-view.service';

describe('SwitchViewService', () => {
  let service: SwitchViewService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SwitchViewService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
