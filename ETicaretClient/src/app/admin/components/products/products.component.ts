import { Component } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpClientService } from '../../../services/common/http-client.service';
import { Create_Product } from '../../../contracts/create_product';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent extends BaseComponent {
  constructor(
    spinner: NgxSpinnerService
    // private httpClientService: HttpClientService
  ) {
    super(spinner);
    this.showSpinner(SpinnerType.BallScaleMultiple);

    // this.httpClientService
    //   .get<Create_Product[]>({
    //     controller: 'products',
    //   })
    //   .subscribe((data) => console.log(data));

    // this.httpClientService
    //   .post(
    //     {
    //       controller: 'products',
    //     },
    //     {
    //       name: 'tekerlek',
    //       stock: 20,
    //       price: 30,
    //     }
    //   )
    //   .subscribe();

    // this.httpClientService
    //   .put(
    //     {
    //       controller: 'products',
    //     },
    //     {
    //       id: '0198603c-4962-7cc7-ba29-8a05df400f48',
    //       name: 'Renkli Kağıt',
    //       stock: 10,
    //       price: 5.5,
    //     }
    //   )
    //   .subscribe();

    // this.httpClientService
    //   .delete(
    //     { controller: 'products' },
    //     '01986069-c2e4-7d2d-94d7-d024b8c16340'
    //   )
    //   .subscribe();
  }
}
