import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplyProposedRatingsComponent } from './reply-proposed-ratings.component';

describe('ReplyProposedRatingsComponent', () => {
  let component: ReplyProposedRatingsComponent;
  let fixture: ComponentFixture<ReplyProposedRatingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReplyProposedRatingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReplyProposedRatingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
