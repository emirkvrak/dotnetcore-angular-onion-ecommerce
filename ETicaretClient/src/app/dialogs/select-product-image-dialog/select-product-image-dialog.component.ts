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
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../base/base.component';

declare var $: any;

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
    private productService: ProductService,
    private spinner: NgxSpinnerService
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
    this.spinner.show(SpinnerType.BallScaleMultiple);
    this.options.queryString = `id=${this.data}`;
    this.images = await this.productService.readeImages(
      this.data as string,
      () => this.spinner.hide(SpinnerType.BallScaleMultiple)
    );
  }

  async deleteImage(imageId: string) {
    this.spinner.show(SpinnerType.BallScaleMultiple);
    await this.productService.deleteImage(this.data as string, imageId, () => {
      this.spinner.hide(SpinnerType.BallScaleMultiple);
      $(this).fadeOut(500);
    });
  }
}

export enum SelectProductImageState {
  Close,
}
