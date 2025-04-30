import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproveRatingCardComponent } from './approve-rating-card.component';

describe('ApproveRatingCardComponent', () => {
  let component: ApproveRatingCardComponent;
  let fixture: ComponentFixture<ApproveRatingCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApproveRatingCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApproveRatingCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
