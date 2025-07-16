import {
  Directive,
  ElementRef,
  HostListener
} from '@angular/core';

@Directive({
  selector: '[appDigitsOnly]'
})
export class DigitsOnlyDirective {
  constructor(private readonly el: ElementRef<HTMLInputElement>) { }

  @HostListener('keydown', ['$event'])
  handleKeyDown(event: KeyboardEvent): void {
    const allowedKeys = [
      'Backspace', 'Tab', 'ArrowLeft', 'ArrowRight', 'Delete'
    ];

    const isDigit = /^\d$/.test(event.key); 

    if (!isDigit && !allowedKeys.includes(event.key)) {
      event.preventDefault();
    }
  }

  @HostListener('paste', ['$event'])
  handlePaste(event: ClipboardEvent): void {
    const pasted = event.clipboardData?.getData('text') ?? '';
    const cleaned = pasted.replace(/\D/g, ''); 

    if (!cleaned) {
      event.preventDefault(); 
      return;
    }

    event.preventDefault();
    const input = this.el.nativeElement;
    const start = input.selectionStart ?? 0;
    const end = input.selectionEnd ?? 0;
    const newValue =
      input.value.substring(0, start) + cleaned + input.value.substring(end);
    input.value = newValue;

    input.dispatchEvent(new Event('input', { bubbles: true }));
  }
}
