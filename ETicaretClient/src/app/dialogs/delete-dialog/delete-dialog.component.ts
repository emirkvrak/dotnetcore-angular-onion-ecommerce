import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-dialog',
  standalone: false,
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.scss',
})
export class DeleteDialogComponent {
  public DeleteState = DeleteState;

  constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>) {}

  close(): void {
    this.dialogRef.close();
  }
}

export enum DeleteState {
  Yes = 'Yes',
  No = 'No',
}
