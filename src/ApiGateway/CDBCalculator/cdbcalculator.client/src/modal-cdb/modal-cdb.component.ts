import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-modal-cdb',
  templateUrl: './modal-cdb.component.html',
  styleUrls: ['./modal-cdb.component.css']
})
export class ModalCdbComponent {
  @Input() grossFormatted: string = '';
  @Input() netFormatted: string = '';
  @Input() isSuccess: boolean = true;
  @Input() message: string = '';

  @Output() closed = new EventEmitter<void>();

  close(): void {
    this.closed.emit();
  }
}
