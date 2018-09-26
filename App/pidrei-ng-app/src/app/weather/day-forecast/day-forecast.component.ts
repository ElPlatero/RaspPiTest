import { Component, OnInit, Input } from '@angular/core';
import { DayForecast } from '../../../shared/classes/day-forecast';
import { WindInformation } from '../../../shared/classes/wind-information';
import { WeatherService } from '../../../shared/services/weather.service';

@Component({
  selector: 'app-day-forecast',
  templateUrl: './day-forecast.component.html',
  styleUrls: ['./day-forecast.component.css']
})
export class DayForecastComponent implements OnInit {
  @Input() public forecast: DayForecast;

  constructor(private _weatherService: WeatherService) { }

  ngOnInit() {
  }
  public getDay(date: Date) {
    const compare = new Date();

    if (date.toDateString() === compare.toDateString()) {
      return 'Heute';
    }
    compare.setDate(compare.getDate() + 1);

    if (date.toDateString() === compare.toDateString()) {
      return 'Morgen';
    }

    compare.setDate(compare.getDate() + 1);

    if (date.toDateString() === compare.toDateString()) {
      return 'Übermorgen';
    }

    const weekdayNames = ['Sonntag', 'Montag', 'Dienstag', 'Mittwoch', 'Donnerstag', 'Freitag', 'Samstag'];
    return weekdayNames[date.getDay()];
  }

  public getWindDirection(wind: WindInformation): string {
    switch (wind.direction) {
      case 0: return 'windstill';
      case 1: return 'rotate(270)';
      case 2: return 'rotate(292.5deg)';
      case 3: return 'rotate(315deg)';
      case 4: return 'rotate(337.5deg)';
      case 5: return 'rotate(0deg)';
      case 6: return 'rotate(22.5deg)';
      case 7: return 'rotate(45deg)';
      case 8: return 'rotate(67.5deg)';
      case 9: return 'rotate(90deg)';
      case 10: return 'rotate(112.5deg)';
      case 11: return 'rotate(135deg)';
      case 12: return 'rotate(157.5deg)';
      case 13: return 'rotate(180deg)';
      case 14: return 'rotate(202.5deg)';
      case 15: return 'rotate(225deg)';
      case 16: return 'rotate(247.5deg)';
    }
    return '';
  }

  public getIcon(sunshine: number, isNight: boolean) {
    return this._weatherService.getIcon(sunshine, isNight);
  }
}
