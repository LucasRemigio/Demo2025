import { TestBed } from '@angular/core/testing';

import { ProductDependenciesService } from './product-dependencies.service';

describe('ProductDependenciesService', () => {
  let service: ProductDependenciesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductDependenciesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
