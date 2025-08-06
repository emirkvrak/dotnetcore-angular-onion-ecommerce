import { Component } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  CustomToastrService,
  ToastrMessageType,
  ToastrPosition,
} from '../../../services/ui/custom-toastr.service';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent extends BaseComponent {
  constructor(
    spinner: NgxSpinnerService
    // private toastrService: CustomToastrService
  ) {
    super(spinner);
    this.showSpinner(SpinnerType.BallScaleMultiple);

    // this.toastrService.message('Ürün başarıyla yüklendi.', 'Başarılı', {
    //   messageType: ToastrMessageType.Success,
    //   position: ToastrPosition.TopRight,
    // });

    // this.toastrService.message('Ürün yüklenirken bir hata oluştu.', 'Hata', {
    //   messageType: ToastrMessageType.Error,
    //   position: ToastrPosition.TopRight,
    // });

    // this.toastrService.message(
    //   'Yeni ürün ekleme özelliği aktif değil.',
    //   'Bilgi',
    //   {
    //     messageType: ToastrMessageType.Info,
    //     position: ToastrPosition.TopRight,
    //   }
    // );

    // this.toastrService.message('Stok miktarı çok düşük!', 'Uyarı', {
    //   messageType: ToastrMessageType.Warning,
    //   position: ToastrPosition.TopRight,
    // });
  }
}
