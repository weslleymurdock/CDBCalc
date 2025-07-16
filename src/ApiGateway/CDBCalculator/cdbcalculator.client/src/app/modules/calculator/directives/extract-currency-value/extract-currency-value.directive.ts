import {
  Directive,
  ElementRef,
  HostListener,
  Output,
  EventEmitter
} from '@angular/core';

@Directive({
  selector: '[appExtractCurrencyValue]'
})
export class AppExtractCurrencyValueDirective {
  @Output() valueExtracted = new EventEmitter<number>();

  constructor(private readonly el: ElementRef<HTMLInputElement>) { }

  @HostListener('input')
  @HostListener('blur')
  handleInput(): void {
    const raw = this.el.nativeElement.value;

    const cleaned = raw
      .replace(/\s/g, '')
      .replace(/[R$\u00A0]/g, '')   // Remove prefixo monetário e espaços não quebráveis
      .replace(/\./g, '')           // Remove separador de milhar
      .replace(/,/g, '.');          // Troca vírgula por ponto

    const value = parseFloat(cleaned);
    this.valueExtracted.emit(isNaN(value) ? 0 : value);
  }
}
