import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DistanceInfoComponent } from './distance-info.component';

describe('DistanceInfoComponent', () => {
  let component: DistanceInfoComponent;
  let fixture: ComponentFixture<DistanceInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DistanceInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DistanceInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
