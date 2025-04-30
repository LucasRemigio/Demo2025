import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectCancelReasonComponent } from './select-cancel-reason.component';

describe('SelectCancelReasonComponent', () => {
  let component: SelectCancelReasonComponent;
  let fixture: ComponentFixture<SelectCancelReasonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectCancelReasonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectCancelReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
