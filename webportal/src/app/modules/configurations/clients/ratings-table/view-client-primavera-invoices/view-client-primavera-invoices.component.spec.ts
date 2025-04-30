import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewClientPrimaveraInvoicesComponent } from './view-client-primavera-invoices.component';

describe('ViewClientPrimaveraInvoicesComponent', () => {
  let component: ViewClientPrimaveraInvoicesComponent;
  let fixture: ComponentFixture<ViewClientPrimaveraInvoicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewClientPrimaveraInvoicesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewClientPrimaveraInvoicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
