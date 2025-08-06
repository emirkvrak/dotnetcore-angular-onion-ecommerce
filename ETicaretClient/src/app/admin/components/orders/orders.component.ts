import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from '../../../base/base.component';

@Component({
  selector: 'app-orders',
  standalone: false,
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss',
})
export class OrdersComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
    this.showSpinner(SpinnerType.BallScaleMultiple);
  }
}
