import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { Create_Product } from '../../../contracts/create_product';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private httpClientService: HttpClientService) {}

  create(
    product: Create_Product,
    successCallback?: () => void,
    errorCallback?: (errorMessage: string) => void
  ): void {
    this.httpClientService
      .post(
        {
          controller: 'products',
        },
        product
      )
      .subscribe({
        next: () => {
          if (successCallback) successCallback();
        },
        error: (errorResponse: HttpErrorResponse) => {
          const errors = errorResponse.error;
          let message = '';

          if (errors && typeof errors === 'object') {
            Object.entries(errors).forEach(([_, value]) => {
              if (Array.isArray(value)) {
                value.forEach((msg) => {
                  message += `${msg}<br>`;
                });
              }
            });
          }

          if (errorCallback) errorCallback(message);
        },
      });
  }
}
