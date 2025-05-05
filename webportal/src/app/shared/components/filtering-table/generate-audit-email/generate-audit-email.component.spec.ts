import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateAuditEmailComponent } from './generate-audit-email.component';

describe('GenerateAuditEmailComponent', () => {
  let component: GenerateAuditEmailComponent;
  let fixture: ComponentFixture<GenerateAuditEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenerateAuditEmailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateAuditEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
