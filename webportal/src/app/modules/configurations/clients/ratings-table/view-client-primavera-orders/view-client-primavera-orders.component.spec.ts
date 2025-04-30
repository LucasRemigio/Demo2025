import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewClientPrimaveraOrdersComponent } from './view-client-primavera-orders.component';

describe('ViewClientPrimaveraOrdersComponent', () => {
  let component: ViewClientPrimaveraOrdersComponent;
  let fixture: ComponentFixture<ViewClientPrimaveraOrdersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewClientPrimaveraOrdersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewClientPrimaveraOrdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
