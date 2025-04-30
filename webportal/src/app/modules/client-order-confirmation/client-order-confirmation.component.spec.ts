import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientOrderConfirmationComponent } from './client-order-confirmation.component';

describe('ClientOrderConfirmationComponent', () => {
  let component: ClientOrderConfirmationComponent;
  let fixture: ComponentFixture<ClientOrderConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientOrderConfirmationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientOrderConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
