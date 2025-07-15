import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  months: number = 1;
  initialValue: number = 0;

  gross: number = 0;
  net: number = 0;

  showModal: boolean = false;
  isSuccess: boolean = true;
  message: string = '';

  constructor(private readonly http: HttpClient) { }

  simulate(): void {
    const formData = new FormData();
    formData.append('Months', this.months.toString());
    formData.append('InitialValue', this.initialValue.toString());

    this.http.post<any>('/gateway/Cdb/Simulate', formData).subscribe({
      next: res => {
        const isValid = isFinite(res?.gross) && isFinite(res?.net);

        if (!isValid || res?.success === false) {
          this.isSuccess = false;
          this.message = res?.message ?? 'O cálculo retornou um valor inválido.';
        } else {
          this.gross = res.gross;
          this.net = res.net;
          this.isSuccess = true;
          this.message = 'Simulação concluída com sucesso.';
        }

        this.showModal = true;
      },
      error: err => {
        console.error('Erro na simulação:', err);
        this.isSuccess = false;
        this.message =
          err?.error?.message ??
          'Erro interno ao processar a simulação. Verifique os valores informados.';
        this.showModal = true;
      }
    });
  }

  formatCurrency(value: number): string {
    return value.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL',
      maximumFractionDigits: 2,
      minimumFractionDigits: 2
    });
  }

  closeModal(): void {
    this.showModal = false;
  }
}
