import { Component } from '@angular/core';
import { CdbSimulationResponse } from './models/cdb.model';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html'
})
export class CalculatorComponent {
  resultData: CdbSimulationResponse | null = null;
  constructor() { }

  handleResult(result: CdbSimulationResponse): void {
    this.resultData = result;
  }

  clearResult(): void {
    this.resultData = null;
  }

}
