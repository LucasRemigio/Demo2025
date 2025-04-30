import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FwdOrderComponent } from './fwd-email.component';

describe('FwdOrderComponent', () => {
    let component: FwdOrderComponent;
    let fixture: ComponentFixture<FwdOrderComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FwdOrderComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FwdOrderComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
