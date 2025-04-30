import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEmailPopupComponent } from './show-email-popup.component';

describe('ShowEmailPopupComponent', () => {
  let component: ShowEmailPopupComponent;
  let fixture: ComponentFixture<ShowEmailPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowEmailPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEmailPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
