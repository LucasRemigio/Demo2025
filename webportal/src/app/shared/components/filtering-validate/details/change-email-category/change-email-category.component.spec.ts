import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeEmailCategoryComponent } from './change-email-category.component';

describe('ChangeEmailCategoryComponent', () => {
  let component: ChangeEmailCategoryComponent;
  let fixture: ComponentFixture<ChangeEmailCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeEmailCategoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeEmailCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
