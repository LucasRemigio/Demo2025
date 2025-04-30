import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPlatformSettingComponent } from './edit-platform-setting.component';

describe('EditPlatformSettingComponent', () => {
  let component: EditPlatformSettingComponent;
  let fixture: ComponentFixture<EditPlatformSettingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditPlatformSettingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPlatformSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
