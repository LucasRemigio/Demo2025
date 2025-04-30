import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-view-transaction',
  templateUrl: './view-transactions.component.html'
})
export class ViewTransactionComponent implements OnInit {
  transaction: any;

  constructor(
    public matDialogRef: MatDialogRef<ViewTransactionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    this.transaction = this.data.transaction;
  }

  close(): void {
    this.matDialogRef.close();
  }
}