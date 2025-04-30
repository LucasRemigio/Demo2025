import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidateTableComponent } from './validate-table.component';

describe('ValidateTableComponent', () => {
  let component: ValidateTableComponent;
  let fixture: ComponentFixture<ValidateTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ValidateTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidateTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
