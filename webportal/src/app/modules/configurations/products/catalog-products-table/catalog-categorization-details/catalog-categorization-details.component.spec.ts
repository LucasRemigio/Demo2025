import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogCategorizationDetailsComponent } from './catalog-categorization-details.component';

describe('CatalogCategorizationDetailsComponent', () => {
  let component: CatalogCategorizationDetailsComponent;
  let fixture: ComponentFixture<CatalogCategorizationDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CatalogCategorizationDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogCategorizationDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
