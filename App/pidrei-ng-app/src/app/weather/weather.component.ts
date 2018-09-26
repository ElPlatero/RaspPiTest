import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../../shared/services/weather.service';
import { WeatherForecast } from '../../shared/classes/weather-forecast';
import { DayForecast } from '../../shared/classes/day-forecast';
import { WindInformation } from '../../shared/classes/wind-information';
import { WeatherConditions } from '../../shared/classes/weather-conditions';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  public forecast: WeatherForecast;
  public currentConditions: WeatherConditions = new WeatherConditions();

  constructor(private _weatherService: WeatherService) {
    this.currentConditions = new WeatherConditions();
  }

  ngOnInit() {
    this.getCurrentConditions();
    this.refreshForecast();
  }

  public getCurrentConditions() {
    this._weatherService.getCurrentConditions().then(p => Object.assign(this.currentConditions, p));
  }

  public refreshForecast(): void {
    this._weatherService.getForecast().then(p => this.forecast = p);
  }

  public getCurrentIcon() {
    return this._weatherService.getIcon(this.currentConditions.sunshine, false);
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

  public getSunshine(forecast: DayForecast): string {
    return  `morgens ${this.toDescription(forecast.morning)}, ` +
            `nachmittags ${this.toDescription(forecast.afternoon)}, ` +
            `nachts ${this.toDescription(forecast.night)}`;
  }

  public toDescription(sunshine: number) {
    switch (sunshine) {
      case 0: return 'wolkenlos';
      case 1: return 'bedeckt';
      case 2: return 'stark bewölkt';
      case 3: return 'leicht bewölkt';
      case 4: return 'Regen';
      case 5: return 'Gewitter';
    }
    return '';
  }

  public getWindDirection(wind: WindInformation): string {
    switch (wind.direction) {
      case 0: return 'windstill';
      case 1: return 'N';
      case 2: return 'NNE';
      case 3: return 'NE';
      case 4: return 'ENE';
      case 5: return 'E';
      case 6: return 'ESE';
      case 7: return 'SE';
      case 8: return 'SSE';
      case 9: return 'S';
      case 10: return 'SSW';
      case 11: return 'SW';
      case 12: return 'WSW';
      case 13: return 'W';
      case 14: return 'WNW';
      case 15: return 'NW';
      case 16: return 'NNW';
    }
    return '';
  }
}
