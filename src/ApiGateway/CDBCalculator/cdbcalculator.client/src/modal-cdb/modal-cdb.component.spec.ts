import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCdbComponent } from './modal-cdb.component';

describe('ModalCdbComponent', () => {
  let component: ModalCdbComponent;
  let fixture: ComponentFixture<ModalCdbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ModalCdbComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCdbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
