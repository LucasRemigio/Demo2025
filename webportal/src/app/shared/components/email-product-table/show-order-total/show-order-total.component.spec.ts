import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowOrderTotalComponent } from './show-order-total.component';

describe('ShowOrderTotalComponent', () => {
  let component: ShowOrderTotalComponent;
  let fixture: ComponentFixture<ShowOrderTotalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowOrderTotalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowOrderTotalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
