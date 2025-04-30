import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericConfirmationPopupComponent } from './generic-confirmation-popup.component';

describe('GenericConfirmationPopupComponent', () => {
  let component: GenericConfirmationPopupComponent;
  let fixture: ComponentFixture<GenericConfirmationPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenericConfirmationPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenericConfirmationPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
