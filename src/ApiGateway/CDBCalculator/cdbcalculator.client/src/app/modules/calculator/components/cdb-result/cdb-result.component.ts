import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CdbSimulationResponse } from '../../models/cdb.model';


@Component({
  selector: 'app-cdb-result',
  templateUrl: './cdb-result.component.html',
  styleUrl: './cdb-result.component.css'
})
export class CdbResultComponent {
  @Input() result: CdbSimulationResponse | null = null;
  @Output() onClear = new EventEmitter<void>();

  formatCurrency(value: number): string {
    return value.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL',
      minimumFractionDigits: 2
    });
  }
}
