import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ElementActionComponent } from './elem-action.component';

describe('AddProjectComponent', () => {
    let component: ElementActionComponent;
    let fixture: ComponentFixture<ElementActionComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [ElementActionComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(ElementActionComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});