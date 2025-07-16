import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CdbResultComponent } from './cdb-result.component';

describe('CdbResultComponent', () => {
  let component: CdbResultComponent;
  let fixture: ComponentFixture<CdbResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CdbResultComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CdbResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
