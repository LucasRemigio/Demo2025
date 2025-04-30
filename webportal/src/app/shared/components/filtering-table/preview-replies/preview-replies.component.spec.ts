import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewRepliesComponent } from './preview-replies.component';

describe('PreviewRepliesComponent', () => {
  let component: PreviewRepliesComponent;
  let fixture: ComponentFixture<PreviewRepliesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreviewRepliesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewRepliesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
