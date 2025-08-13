import { Component, EventEmitter, Output } from '@angular/core';
import { ProductService } from '../../../../services/common/models/product.service';
import { Create_Product } from '../../../../contracts/create_product';
import { BaseComponent, SpinnerType } from '../../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  AlertifyService,
  MessageType,
  Position,
} from '../../../../services/admin/alertify.service';
import { FileUploadOptions } from '../../../../services/common/file-upload/file-upload.component';

@Component({
  selector: 'app-create',
  standalone: false,
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss',
})
export class CreateComponent extends BaseComponent {
  constructor(
    spinner: NgxSpinnerService,
    private productService: ProductService,
    private alertify: AlertifyService
  ) {
    super(spinner);
  }

  @Output() createdProduct: EventEmitter<Create_Product> = new EventEmitter();
  @Output() fileUploadOptions: Partial<FileUploadOptions> = {
    action: 'upload',
    controller: 'products',
    explanation: 'Resimleri sürükleyin veya seçin ...',
    isAdminPage: true,
    accept: '.png, .jpg, .jpeg',
  };

  create(
    nameInput: HTMLInputElement,
    stockInput: HTMLInputElement,
    priceInput: HTMLInputElement
  ) {
    this.showSpinner(SpinnerType.BallScaleMultiple);

    const create_product = new Create_Product();
    create_product.name = nameInput.value;
    create_product.stock = Number(stockInput.value);
    create_product.price = Number(priceInput.value);

    this.productService.create(
      create_product,
      () => {
        this.hideSpinner(SpinnerType.BallScaleMultiple);
        this.alertify.message('Ürün başarıyla eklendi.', {
          messageType: MessageType.Success,
          position: Position.TopRight,
        });
        this.createdProduct.emit(create_product);
      },
      (errorMessage: string) => {
        this.hideSpinner(SpinnerType.BallScaleMultiple);
        this.alertify.message(errorMessage, {
          dismissOthers: true,
          messageType: MessageType.Error,
          position: Position.TopRight,
        });
      }
    );
  }
}
