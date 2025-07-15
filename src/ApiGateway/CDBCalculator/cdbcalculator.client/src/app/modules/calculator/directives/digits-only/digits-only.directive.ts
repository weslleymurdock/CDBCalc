import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appDigitsOnly]'
})
export class DigitsOnlyDirective {
  private readonly navigationKeys = ['Backspace', 'ArrowLeft', 'ArrowRight', 'Tab'];

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    const isDigit = /^\d$/.test(event.key);
    if (!isDigit && !this.navigationKeys.includes(event.key)) {
      event.preventDefault();
    }
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent): void {
    const pasted = event.clipboardData?.getData('text') ?? '';
    if (!/^\d+$/.test(pasted.replace(/\D/g, ''))) {
      event.preventDefault();
    }
  }
}
