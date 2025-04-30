import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogProductsTableComponent } from './catalog-products-table.component';

describe('CatalogProductsTableComponent', () => {
  let component: CatalogProductsTableComponent;
  let fixture: ComponentFixture<CatalogProductsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CatalogProductsTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CatalogProductsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
