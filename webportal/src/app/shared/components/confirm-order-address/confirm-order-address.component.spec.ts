import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmOrderAddressComponent } from './confirm-order-address.component';

describe('ConfirmOrderAddressComponent', () => {
  let component: ConfirmOrderAddressComponent;
  let fixture: ComponentFixture<ConfirmOrderAddressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfirmOrderAddressComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmOrderAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
