import { Component, Inject, OnInit, Output } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { FileUploadOptions } from '../../services/common/file-upload/file-upload.component';
import { ProductService } from '../../services/common/models/product.service';
import { List_Product_Image } from '../../contracts/list_product_images';

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
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageState | string,
    private productService: ProductService
  ) {
    super(dialogRef);
  }

  options: Partial<FileUploadOptions> = {
    accept: '.png, .jpg, .jpeg, .gif',
    action: 'upload',
    controller: 'products',
    explanation: 'Ürün resmini seçin veya buraya sürükleyin...',
    isAdminPage: true,
    queryString: '',
  };

  images: List_Product_Image[];

  async ngOnInit() {
    this.options.queryString = `id=${this.data}`;
    this.images = await this.productService.readeImages(this.data as string);
  }
}

export enum SelectProductImageState {
  Close,
}
