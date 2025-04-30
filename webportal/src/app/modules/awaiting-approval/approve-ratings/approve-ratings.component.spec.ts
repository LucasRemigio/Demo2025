import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproveRatingsComponent } from './approve-ratings.component';

describe('ApproveRatingsComponent', () => {
  let component: ApproveRatingsComponent;
  let fixture: ComponentFixture<ApproveRatingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApproveRatingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApproveRatingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
