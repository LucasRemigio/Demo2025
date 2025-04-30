import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailFwdListComponent } from './email-fwd-list.component';

describe('EmailFwdListComponent', () => {
  let component: EmailFwdListComponent;
  let fixture: ComponentFixture<EmailFwdListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailFwdListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailFwdListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
