import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { WeatherForecast } from '../classes/weather-forecast';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private _httpClient: HttpClient) { }

  public getForecast(): Promise<WeatherForecast> {
    const uri = environment.apiBaseUrl + '/weather/forecast';
    return this
      ._httpClient
      .get(uri)
      .pipe(map((p: WeatherForecast) => {
        const result = new WeatherForecast();
        Object.assign(result, p);
        result.today.date = new Date(result.today.date);
        result.tommorrow.date = new Date(result.tommorrow.date);
        result.dayAfterTommorrow.date = new Date(result.dayAfterTommorrow.date);
        return result;
      }))
      .toPromise<WeatherForecast>();
  }
}
