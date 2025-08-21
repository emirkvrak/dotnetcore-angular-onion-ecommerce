import { Component, Inject } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-file-upload-dialog',
  standalone: false,
  templateUrl: './file-upload-dialog.component.html',
  styleUrl: './file-upload-dialog.component.scss',
})
export class FileUploadDialogComponent extends BaseDialog<FileUploadDialogComponent> {
  public FileUploadDialogState = FileUploadDialogState;
  constructor(
    dialogRef: MatDialogRef<FileUploadDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FileUploadDialogState
  ) {
    super(dialogRef);
  }
}

export enum FileUploadDialogState {
  Yes = 'Yes',
  No = 'No',
}
