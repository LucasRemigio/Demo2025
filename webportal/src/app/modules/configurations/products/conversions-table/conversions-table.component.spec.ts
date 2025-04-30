import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConversionsTableComponent } from './conversions-table.component';

describe('ConversionsTableComponent', () => {
  let component: ConversionsTableComponent;
  let fixture: ComponentFixture<ConversionsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConversionsTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConversionsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
