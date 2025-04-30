import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCancelReasonComponent } from './edit-cancel-reason.component';

describe('EditCancelReasonComponent', () => {
  let component: EditCancelReasonComponent;
  let fixture: ComponentFixture<EditCancelReasonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCancelReasonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCancelReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
