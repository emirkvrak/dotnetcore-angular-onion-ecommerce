import { Component } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService) {
    super(spinner);
    this.showSpinner(SpinnerType.BallScaleMultiple);
  }
}
