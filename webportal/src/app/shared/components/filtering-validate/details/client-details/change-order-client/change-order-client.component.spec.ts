import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeOrderClientComponent } from './change-order-client.component';

describe('ChangeOrderClientComponent', () => {
  let component: ChangeOrderClientComponent;
  let fixture: ComponentFixture<ChangeOrderClientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeOrderClientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeOrderClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
