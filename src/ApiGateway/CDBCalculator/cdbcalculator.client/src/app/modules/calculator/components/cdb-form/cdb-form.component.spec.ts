import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CdbFormComponent } from './cdb-form.component';

describe('CdbFormComponent', () => {
  let component: CdbFormComponent;
  let fixture: ComponentFixture<CdbFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CdbFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CdbFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
