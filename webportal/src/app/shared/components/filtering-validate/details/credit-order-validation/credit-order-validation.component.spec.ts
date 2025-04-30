import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditOrderValidationComponent } from './credit-order-validation.component';

describe('CreditOrderValidationComponent', () => {
  let component: CreditOrderValidationComponent;
  let fixture: ComponentFixture<CreditOrderValidationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreditOrderValidationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreditOrderValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
