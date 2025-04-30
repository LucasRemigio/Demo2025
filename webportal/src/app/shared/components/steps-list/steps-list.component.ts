import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
    selector: 'app-steps-list',
    templateUrl: './steps-list.component.html',
    styleUrls: ['./steps-list.component.scss'],
})
export class StepsListComponent implements OnInit {
    @Input() steps: { order: number; title: string; subtitle: string }[];
    @Input() currentStep: number;

    @Output() stepChange = new EventEmitter<number>();
    constructor() {}

    ngOnInit(): void {}

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    goToStep(step: number): void {
        this.stepChange.emit(step);
    }
}
