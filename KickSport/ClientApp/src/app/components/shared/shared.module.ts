import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';

import { sharedComponents } from '.';
import { MDBBootstrapModule } from 'angular-bootstrap-md';

@NgModule({
  declarations: [
    ...sharedComponents
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    RouterModule,
    MDBBootstrapModule.forRoot()
  ],
  schemas: [NO_ERRORS_SCHEMA],
  exports: [
    ...sharedComponents
  ]
})
export class SharedModule { }
