import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilteringTableComponent } from './filtering-table.component';

describe('FilteringTableComponent', () => {
  let component: FilteringTableComponent;
  let fixture: ComponentFixture<FilteringTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilteringTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FilteringTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
