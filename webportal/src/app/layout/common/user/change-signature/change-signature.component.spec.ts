import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeSignatureComponent } from './change-signature.component';

describe('ChangeSignatureComponent', () => {
  let component: ChangeSignatureComponent;
  let fixture: ComponentFixture<ChangeSignatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeSignatureComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeSignatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
