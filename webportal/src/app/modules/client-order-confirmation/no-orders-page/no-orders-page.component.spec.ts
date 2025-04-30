import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoOrdersPageComponent } from './no-orders-page.component';

describe('NoOrdersPageComponent', () => {
  let component: NoOrdersPageComponent;
  let fixture: ComponentFixture<NoOrdersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NoOrdersPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NoOrdersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
