import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FritzDevicesComponent } from './fritz-devices.component';

describe('FritzDevicesComponent', () => {
  let component: FritzDevicesComponent;
  let fixture: ComponentFixture<FritzDevicesComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ FritzDevicesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FritzDevicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
