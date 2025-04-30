import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailReplyFormComponent } from './email-reply-form.component';

describe('EmailReplyFormComponent', () => {
  let component: EmailReplyFormComponent;
  let fixture: ComponentFixture<EmailReplyFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailReplyFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailReplyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
