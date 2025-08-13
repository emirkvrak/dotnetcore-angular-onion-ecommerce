import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { Create_Product } from '../../../contracts/create_product';
import { HttpErrorResponse } from '@angular/common/http';
import { List_Product } from '../../../contracts/list_product';
import { firstValueFrom, Observable } from 'rxjs';

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

  async read(
    page: number = 0,
    size: number = 5,
    successCallback?: () => void,
    errorCallback?: (errorMessage: string) => void
  ): Promise<{ totalCount: number; products: List_Product[] }> {
    const promiseData: Promise<{
      totalCount: number;
      products: List_Product[];
    }> = this.httpClientService
      .get<{ totalCount: number; products: List_Product[] }>({
        controller: 'products',
        queryString: `page=${page}&size=${size}`,
      })
      .toPromise();

    promiseData
      .then((d) => successCallback())
      .catch((errorResponse: HttpErrorResponse) =>
        errorCallback(errorResponse.message)
      );

    return await promiseData;
  }

  async delete(id: string) {
    const deleteObservable: Observable<List_Product> =
      this.httpClientService.delete<List_Product>(
        {
          controller: 'products',
        },
        id
      );

    var a = await firstValueFrom(deleteObservable);
  }
}
