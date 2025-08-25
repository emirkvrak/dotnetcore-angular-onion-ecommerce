import { Component, Inject, OnInit, Output } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { FileUploadOptions } from '../../services/common/file-upload/file-upload.component';

@Component({
  selector: 'app-select-product-image-dialog',
  standalone: false,
  templateUrl: './select-product-image-dialog.component.html',
  styleUrl: './select-product-image-dialog.component.scss',
})
export class SelectProductImageDialogComponent
  extends BaseDialog<SelectProductImageDialogComponent>
  implements OnInit
{
  constructor(
    dialogRef: MatDialogRef<SelectProductImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageState | string
  ) {
    super(dialogRef);
  }

  @Output() options: Partial<FileUploadOptions>;

  ngOnInit() {
    this.options = {
      accept: '.png, .jpg, .jpeg, .gif',
      action: 'upload',
      controller: 'products',
      explanation: 'Ürün resmini seçin veya buraya sürükleyin...',
      isAdminPage: true,
      queryString: `id=${this.data}`,
    };
  }
}

export enum SelectProductImageState {
  Close,
}
