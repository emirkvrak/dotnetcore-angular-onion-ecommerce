import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketsModule } from './baskets/baskets.module';
import { HomeModule } from './home/home.module';
import { ProductsModule } from './products/products.module';
import { ComponentsComponent } from './components.component';

@NgModule({
  declarations: [ComponentsComponent],
  imports: [CommonModule, BasketsModule, HomeModule, ProductsModule],
  exports: [ComponentsComponent], //bunları sor hem declarations hem export kendim şimdi yazdım
})
export class ComponentsModule {}
