import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DayForecastComponent } from './day-forecast.component';

describe('DayForecastComponent', () => {
  let component: DayForecastComponent;
  let fixture: ComponentFixture<DayForecastComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DayForecastComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DayForecastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
