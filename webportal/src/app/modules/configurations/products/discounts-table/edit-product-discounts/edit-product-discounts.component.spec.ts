import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProductDiscountsComponent } from './edit-product-discounts.component';

describe('EditProductDiscountsComponent', () => {
  let component: EditProductDiscountsComponent;
  let fixture: ComponentFixture<EditProductDiscountsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProductDiscountsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProductDiscountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
