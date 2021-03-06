import { CommonModule } from '@angular/common'
import { FontAwesomeModule } from '../../../../node_modules/@fortawesome/angular-fontawesome'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core'
import { NgxPaginationModule } from 'ngx-pagination'
import { ProductsRoutingModule } from './products.routing'
import { SharedModule } from '../shared/shared.module'

import { productComponents } from '.'
import { ProductDeleteModalComponent } from './product-delete-modal/product-delete-modal.component'
import { ReviewDeleteModalComponent } from './review-delete-modal/review-delete-modal.component'
import { MDBBootstrapModule } from 'angular-bootstrap-md'

@NgModule({
  declarations: [
    ...productComponents
  ],
  imports: [
    CommonModule,
    NgxPaginationModule,
    FontAwesomeModule,
    FormsModule,
    ProductsRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    MDBBootstrapModule.forRoot(),
  ],
  schemas: [NO_ERRORS_SCHEMA],
  exports: [
    ...productComponents
  ],
  entryComponents: [
    ProductDeleteModalComponent,
    ReviewDeleteModalComponent
  ]
})
export class ProductsModule { }
