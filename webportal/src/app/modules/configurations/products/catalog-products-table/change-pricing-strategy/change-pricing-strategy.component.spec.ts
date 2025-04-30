import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangePricingStrategyComponent } from './change-pricing-strategy.component';

describe('ChangePricingStrategyComponent', () => {
  let component: ChangePricingStrategyComponent;
  let fixture: ComponentFixture<ChangePricingStrategyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangePricingStrategyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangePricingStrategyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
