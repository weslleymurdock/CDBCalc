import { Directive, HostListener, Output, EventEmitter } from '@angular/core';

@Directive({
  selector: '[appCurrencyInput]'
})
export class CurrencyInputDirective {
  @Output() valueChanged = new EventEmitter<number>();

  private raw = '';

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    const input = event.target as HTMLInputElement;
    const isDigit = /^\d$/.test(event.key);
    const allowed = ['Backspace', 'ArrowLeft', 'ArrowRight', 'Tab'];

    if (!isDigit && !allowed.includes(event.key)) {
      event.preventDefault();
      return;
    }

    if (isDigit) {
      this.raw += event.key;
      event.preventDefault();
      this.updateValue(input);
    }

    if (event.key === 'Backspace') {
      this.raw = this.raw.slice(0, -1);
      event.preventDefault();
      this.updateValue(input);
    }
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent): void {
    const pasted = event.clipboardData?.getData('text') ?? '';
    const clean = pasted.replace(/\D/g, '');
    if (!/^\d+$/.test(clean)) {
      event.preventDefault();
      return;
    }

    this.raw = clean;
    const input = event.target as HTMLInputElement;
    this.updateValue(input);
    event.preventDefault();
  }

  private updateValue(input: HTMLInputElement): void {
    const numeric = Number(this.raw || '0') / 100;

    input.value = numeric.toLocaleString('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
      useGrouping: true
    });

    this.valueChanged.emit(numeric);
  }
}
