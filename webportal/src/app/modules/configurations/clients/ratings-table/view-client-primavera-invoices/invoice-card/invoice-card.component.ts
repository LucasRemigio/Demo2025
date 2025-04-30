import { Component, Input, OnInit } from '@angular/core';
import { PrimaveraInvoice } from '../../../clients.types';

export const MY_FORMATS = {
    parse: {
        dateInput: 'LL',
    },
    display: {
        dateInput: 'DD-MM-YYYY',
        monthYearLabel: 'YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'YYYY',
    },
};
@Component({
    selector: 'app-invoice-card',
    templateUrl: './invoice-card.component.html',
    styleUrls: ['./invoice-card.component.scss'],
})
export class InvoiceCardComponent implements OnInit {
    @Input() invoice: PrimaveraInvoice;

    msToDays = 1000 * 60 * 60 * 24;
    timeToPay: number = 0;
    daysRelativeToDueDate: number = 0;

    isOverdue: boolean = false;

    constructor() {}

    ngOnInit(): void {
        this.timeToPay = this.calculateTimeToPay(
            this.invoice.dataDoc,
            this.invoice.dataLiquidacao
        );
        this.daysRelativeToDueDate = this.calculateDaysRelativeToDueDate(
            this.invoice.dataVencimento,
            this.invoice.dataLiquidacao
        );
        this.checkIfOverdue();
    }

    calculateTimeToPay(dataDoc: string, dataLiquidacao: string): number {
        if (!dataDoc || !dataLiquidacao) {
            return 0;
        }

        const docDate = new Date(dataDoc);
        const liquidationDate = new Date(dataLiquidacao);

        // Calculate the difference in milliseconds
        const timeDiff = liquidationDate.getTime() - docDate.getTime();

        return Math.round(timeDiff / this.msToDays);
    }

    calculateDaysRelativeToDueDate(
        dataVencimento: string,
        dataLiquidacao: string
    ): number {
        if (!dataVencimento || !dataLiquidacao) {
            return 0;
        }

        const dueDate = new Date(dataVencimento);
        const liquidationDate = new Date(dataLiquidacao);

        // Calculate the difference in milliseconds
        const timeDiff = liquidationDate.getTime() - dueDate.getTime();

        return Math.round(timeDiff / this.msToDays);
    }

    private checkIfOverdue(): void {
        if (!this.invoice.dataVencimento) {
            return;
        }

        // Compare due date with current date
        const dueDate = new Date(this.invoice.dataVencimento);
        const today = new Date();

        // Clear time portion for accurate date comparison
        dueDate.setHours(0, 0, 0, 0);
        today.setHours(0, 0, 0, 0);

        // Invoice is overdue if:
        // 1. Due date is in the past AND
        // 2. There is still a pending amount
        this.isOverdue = dueDate < today && this.invoice.valorPendente > 0;
    }
}
