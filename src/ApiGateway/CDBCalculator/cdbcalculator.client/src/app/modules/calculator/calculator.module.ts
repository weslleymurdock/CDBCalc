import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

import { CalculatorComponent } from './calculator.component';
import { CdbFormComponent } from './components/cdb-form/cdb-form.component';
import { CdbResultComponent } from './components/cdb-result/cdb-result.component';
import { CurrencyInputDirective } from './directives/currency-input/currency-input.directive';
import { DigitsOnlyDirective } from './directives/digits-only/digits-only.directive';

@NgModule({
  declarations: [
    CalculatorComponent,
    CdbFormComponent,
    CdbResultComponent,
    CurrencyInputDirective,
    DigitsOnlyDirective
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [CalculatorComponent]
})
export class CalculatorModule { }
