import { Component, Output, EventEmitter } from '@angular/core';
import { CdbService } from '../../services/cdb.service';
import { CdbSimulationRequest, CdbSimulationResponse } from '../../models/cdb.model';


@Component({
  selector: 'app-cdb-form',
  templateUrl: './cdb-form.component.html',
  styleUrl: './cdb-form.component.css'
})
export class CdbFormComponent {
  months: number = 1;
  initialValue: number = 0;
  initialValueErrorMessage = '';

  @Output() onSimulate = new EventEmitter<CdbSimulationResponse>();

  constructor(private readonly cdbService: CdbService) { }

  simulate(): void {
    const request: CdbSimulationRequest = {
      months: this.months,
      initialValue: this.initialValue
    };

    if (isNaN(request.initialValue) || request.initialValue < 0.01) {
      this.initialValueErrorMessage = 'Informe um valor monetário maior que R$ 0,01.';
      return;
    }

    this.initialValueErrorMessage = '';

    this.cdbService.simulate(request).subscribe({
      next: res => this.onSimulate.emit(res),
      error: err => this.onSimulate.emit({
        success: false,
        message: err?.error?.message ?? 'Erro ao processar a simulação.',
        gross: 0,
        net: 0,
        statusCode: 0
      })
    });
  }
}
