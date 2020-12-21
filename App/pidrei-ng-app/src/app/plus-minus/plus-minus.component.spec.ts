import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PlusMinusComponent } from './plus-minus.component';

describe('PlusMinusComponent', () => {
  let component: PlusMinusComponent;
  let fixture: ComponentFixture<PlusMinusComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PlusMinusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlusMinusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
