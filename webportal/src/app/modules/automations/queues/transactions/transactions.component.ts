import { Component, OnInit, Input, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QueuesService } from '../queues.service';
import { Transactions } from '../transactions/transactions.types';
import { Subject, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';
import { TransactionsService } from './transactions.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ViewTransactionComponent } from './view-transactions/view-transactions.component';
import { EditTransactions } from './edit-transaction/edit-transactions.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FuseConfirmationService } from '@fuse/services/confirmation';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styles: [
    /* language=SCSS */
    `
        .transactions-grid {
            grid-template-columns: 20% 16% 16% 16% 16% 16%;
        }
    `,
  ],

  encapsulation: ViewEncapsulation.None,
})

export class TransactionsComponent implements OnInit {

  statusList = [
    { id: "1", name: 'Active' },
    { id: "2", name: 'Inactive' },
    { id: "3", name: 'Success' },
    { id: "4", name: 'Insuccess' },
    { id: "5", name: 'Error' },
    { id: "6", name: 'Reprocessed' },
    { id: "7", name: 'Processing' },
    { id: "8", name: 'Removed' },
    { id: "9", name: 'New' }
  ];

  queueId: string;
  transactions$: Observable<Transactions[]>;
  transactions: Transactions[] = [];
  flashMessage: 'success' | 'error' | null = null;
  flashMessageText: string;
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  isLoading: boolean = false;
  public gridData: Transactions[] = [];
  public dialog: MatDialog;

  constructor(
    private _changeDetectorRef: ChangeDetectorRef,
    private _transactionsService: TransactionsService,
    private _formBuilder: FormBuilder,
    private _fuseConfirmationService: FuseConfirmationService,
    private readonly translocoService: TranslocoService,
    private _matDialog: MatDialog,
    private route: ActivatedRoute // Inject ActivatedRoute
  ) { }

  ngOnInit(): void {
      this.queueId = this.route.snapshot.paramMap.get('id');
      const queueId = this.queueId;

      this.getTransactions(queueId);

  }

//   ngOnDestroy(): void {
//     // Unsubscribe from all subscriptions
//     this._unsubscribeAll.next();
//     this._unsubscribeAll.complete();
// }

state: any = {
    skip: 0,
    take: 10,
    sort: [],
};

public pagesizes = [
    {
        text: '10',
        value: 10,
    },

    {
        text: '25',
        value: 25,
    },

    {
        text: '50',
        value: 50,
    },
];

  closeDetails(transaction: any): void {
    transaction.showDetails = false;
  }

  getTransactions(queueId: string): void {
    if (!queueId) {
      return;
    }

    this.isLoading = true; // Set loading indicator
    this._transactionsService.getTransactionsByQueueId(queueId).subscribe(
      (response) => {
        this.isLoading = false;

        // alert(JSON.stringify(response));

        if (response && response.transactions) {
          this.transactions = response.transactions;
          this.transactions$ = of(this.transactions);
          this.gridData = response.transactions;
        }

        this._changeDetectorRef.markForCheck();
      },
      (error: any) => {
        this.isLoading = false; // Turn off loading indicator on error
        this.showFlashMessage('error', this.translocoService.translate('error-loading-list', {}));
        this._changeDetectorRef.markForCheck();
      }
    );
  }

  editTransaction(transaction: Transactions): void {
    const dialogConfig: MatDialogConfig = {
      maxHeight: '80vh',
      minWidth: '70vh',
      data: {
        transaction: transaction,
        statusList: this.statusList
      }
    };

    // alert(JSON.stringify(transaction));

    const dialogRef = this._matDialog.open(EditTransactions, dialogConfig);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.isLoading = true;

        // alert(JSON.stringify(result));

        this._transactionsService.editTransaction(
          result.id,
          result.status_id,
          result.started,
          result.ended,
          result.exception,
          result.output_data
        ).subscribe((response) => {
          this.isLoading = false;
          if (response && response.result_code > 0) {
            this.showFlashMessage(
              'success',
              this.translocoService.translate('Scripts.queue-edit-success', {})
            );
            this.getTransactions(this.queueId);
          } else {
            this.showFlashMessage(
              'error',
              this.translocoService.translate('Scripts.queue-edit-error', {})
            );
          }
          this._changeDetectorRef.markForCheck();
        });
      }
    });
  }

  removeTransaction(id: string) {
      // Open the confirmation dialog
      const confirmation = this._fuseConfirmationService.open({
        title: this.translocoService.translate('Scripts.delete-transaction', {}),
        message: this.translocoService.translate('Scripts.delete-transaction-text', {}),
        actions: {
            confirm: {
                label: this.translocoService.translate('delete', {}),
            },

            cancel: {
                label: this.translocoService.translate('cancel', {}),
            },
        },
    });

    // Subscribe to the confirmation dialog closed action
    confirmation.afterClosed().subscribe((result) => {
      // If the confirm button pressed...
      if (result === 'confirmed') {
          this._transactionsService.removeTransaction(id).subscribe(
              (response) => {
                  this.isLoading = false;

                  if (response && response.result_code > 0) {
                      this.showFlashMessage(
                          'success',
                          this.translocoService.translate('Scripts.delete-transaction-success', {})
                      );
                  } else {
                      this.showFlashMessage(
                          'error',
                          this.translocoService.translate('Scripts.delete-transaction-error', {})
                      );
                  }

                  this.getTransactions(this.queueId);
              },
              (err) => {
                  this.isLoading = false;
                  this.showFlashMessage(
                      'error',
                      this.translocoService.translate(
                          'Scripts.delete-transaction-error',
                          {}
                      )
                  );
              }
          );
      }
  });

    }

    openViewTransaction(transaction: Transactions): void {
      const dialogConfig: MatDialogConfig = {
          maxHeight: '100vh',
          minWidth: '60vh',
          data: { transaction },
      };

      const dialogRef = this._matDialog.open(ViewTransactionComponent, dialogConfig);

      dialogRef.afterClosed().subscribe((result) => {
      });
  }

    /**
     * Show flash message
     */
    showFlashMessage(type: 'success' | 'error', textMsg: string): void {
      // Show the message
      this.flashMessage = type;
      this.flashMessageText = textMsg;

      // Mark for check
      this._changeDetectorRef.markForCheck();

      // Hide it after 3 seconds
      setTimeout(() => {
          this.flashMessage = null;
          this.flashMessageText = null;

          // Mark for check
          this._changeDetectorRef.markForCheck();
      }, 5000);
    }
}


