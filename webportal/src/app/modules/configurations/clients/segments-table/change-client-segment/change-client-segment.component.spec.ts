import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeClientSegmentComponent } from './change-client-segment.component';

describe('ChangeClientSegmentComponent', () => {
  let component: ChangeClientSegmentComponent;
  let fixture: ComponentFixture<ChangeClientSegmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeClientSegmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeClientSegmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
