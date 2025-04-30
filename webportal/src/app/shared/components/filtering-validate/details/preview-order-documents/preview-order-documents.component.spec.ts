import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewOrderDocumentsComponent } from './preview-order-documents.component';

describe('PreviewOrderDocumentsComponent', () => {
  let component: PreviewOrderDocumentsComponent;
  let fixture: ComponentFixture<PreviewOrderDocumentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreviewOrderDocumentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewOrderDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
