import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountsManagmentComponent } from './accounts-managment.component';

describe('AccountsManagmentComponent', () => {
  let component: AccountsManagmentComponent;
  let fixture: ComponentFixture<AccountsManagmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountsManagmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountsManagmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
