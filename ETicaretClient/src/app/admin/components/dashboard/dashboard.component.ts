import { Component } from '@angular/core';
import {
  AlertifyService,
  MessageType,
  Position,
} from '../../../services/admin/alertify.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from '../../../base/base.component';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent extends BaseComponent {
  constructor(private alertify: AlertifyService, spinner: NgxSpinnerService) {
    super(spinner);
    this.showSpinner(SpinnerType.BallScaleMultiple);
  }

  m() {
    this.alertify.message('Merhaba ', {
      messageType: MessageType.Success,
      position: Position.BottomLeft,
      delay: 10,
      dismissOthers: false,
    });
  }

  d() {
    this.alertify.dismiss();
  }
}
