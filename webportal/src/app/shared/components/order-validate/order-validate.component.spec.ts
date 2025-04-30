import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderValidateComponent } from './order-validate.component';

describe('OrderValidateComponent', () => {
  let component: OrderValidateComponent;
  let fixture: ComponentFixture<OrderValidateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderValidateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderValidateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
