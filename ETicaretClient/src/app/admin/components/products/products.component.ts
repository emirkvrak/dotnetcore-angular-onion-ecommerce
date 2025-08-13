import { Component, ViewChild } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Create_Product } from '../../../contracts/create_product';
import { ListComponent } from './list/list.component';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
    this.showSpinner(SpinnerType.BallScaleMultiple);
  }
  @ViewChild(ListComponent) listComponent: ListComponent;

  createdProduct(createdProduct: Create_Product) {
    this.listComponent.getProducts();
  }
}
