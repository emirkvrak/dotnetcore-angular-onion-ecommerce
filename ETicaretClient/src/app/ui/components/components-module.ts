import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Components } from './components';
import { BasketsModule } from './baskets/baskets-module';
import { HomeModule } from './home/home-module';
import { ProductsModule } from './products/products-module';

@NgModule({
  declarations: [Components],
  imports: [CommonModule, ProductsModule, BasketsModule, HomeModule],
})
export class ComponentsModule {}
