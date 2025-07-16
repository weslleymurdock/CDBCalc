import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { AppExtractCurrencyValueDirective } from './directives/extract-currency-value/extract-currency-value.directive'; 
import { CalculatorComponent } from './calculator.component';
import { CdbFormComponent } from './components/cdb-form/cdb-form.component';
import { CdbResultComponent } from './components/cdb-result/cdb-result.component'; 
import { DigitsOnlyDirective } from './directives/digits-only/digits-only.directive';

@NgModule({
  declarations: [
    AppExtractCurrencyValueDirective,  
    CalculatorComponent,
    CdbFormComponent,
    CdbResultComponent, 
    DigitsOnlyDirective,
  ],
  imports: [
    CommonModule,
    CurrencyMaskModule,
    FormsModule
  ],
  exports: [CalculatorComponent]
})
export class CalculatorModule { }
