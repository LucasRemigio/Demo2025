import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditClientRatingComponent } from './edit-client-rating.component';

describe('EditClientRatingComponent', () => {
  let component: EditClientRatingComponent;
  let fixture: ComponentFixture<EditClientRatingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditClientRatingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditClientRatingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
