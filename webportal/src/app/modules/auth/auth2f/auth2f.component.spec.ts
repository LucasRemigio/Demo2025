import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Auth2FComponent } from './auth2f.component';

describe('TokenAuthComponent', () => {
  let component: Auth2FComponent;
  let fixture: ComponentFixture<Auth2FComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Auth2FComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Auth2FComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
