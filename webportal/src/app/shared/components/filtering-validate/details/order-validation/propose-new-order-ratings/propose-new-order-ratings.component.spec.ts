import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProposeNewOrderRatingsComponent } from './propose-new-order-ratings.component';

describe('ProposeNewOrderRatingsComponent', () => {
  let component: ProposeNewOrderRatingsComponent;
  let fixture: ComponentFixture<ProposeNewOrderRatingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProposeNewOrderRatingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProposeNewOrderRatingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
