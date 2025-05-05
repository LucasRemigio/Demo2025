import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAllClientRatingsComponent } from './edit-all-client-ratings.component';

describe('EditAllClientRatingsComponent', () => {
  let component: EditAllClientRatingsComponent;
  let fixture: ComponentFixture<EditAllClientRatingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditAllClientRatingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAllClientRatingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
