import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailProductTableComponent } from './email-product-table.component';

describe('EmailProductTableComponent', () => {
  let component: EmailProductTableComponent;
  let fixture: ComponentFixture<EmailProductTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailProductTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailProductTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
