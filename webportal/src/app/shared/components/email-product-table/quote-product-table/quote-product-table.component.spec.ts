import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuoteProductTableComponent } from './quote-product-table.component';

describe('QuoteProductTableComponent', () => {
  let component: QuoteProductTableComponent;
  let fixture: ComponentFixture<QuoteProductTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuoteProductTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteProductTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
