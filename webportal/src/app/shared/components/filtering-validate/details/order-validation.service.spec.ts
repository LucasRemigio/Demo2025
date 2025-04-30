import { TestBed } from '@angular/core/testing';

import { OrderValidationService } from './order-validation.service';

describe('OrderValidationService', () => {
  let service: OrderValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrderValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
