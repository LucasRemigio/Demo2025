import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidateCardsComponent } from './validate-cards.component';

describe('ValidateCardsComponent', () => {
  let component: ValidateCardsComponent;
  let fixture: ComponentFixture<ValidateCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ValidateCardsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidateCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
