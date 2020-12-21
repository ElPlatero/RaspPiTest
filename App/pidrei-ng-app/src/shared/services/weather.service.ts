import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { WeatherForecast } from '../classes/weather-forecast';
import { map } from 'rxjs/operators';
import { WeatherConditions } from '../classes/weather-conditions';
import { DomSanitizer } from '@angular/platform-browser';
import { MatIconRegistry } from '@angular/material/icon';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private _httpClient: HttpClient, iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
    iconRegistry.addSvgIcon('sunshine-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_sunshine_night.svg'));
    iconRegistry.addSvgIcon('sunshine', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_sunshine.svg'));
    iconRegistry.addSvgIcon('overcast-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_overcast_night.svg'));
    iconRegistry.addSvgIcon('overcast', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_overcast.svg'));
    iconRegistry.addSvgIcon('cloudy-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_cloudy_night.svg'));
    iconRegistry.addSvgIcon('cloudy', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_cloudy.svg'));
    iconRegistry.addSvgIcon('partlycloudy-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_partlycloudy_night.svg'));
    iconRegistry.addSvgIcon('partlycloudy', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_partlycloudy.svg'));
    iconRegistry.addSvgIcon('rain-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_rain_night.svg'));
    iconRegistry.addSvgIcon('rain', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_rain.svg'));
    iconRegistry.addSvgIcon('thunderstorm-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_thunderstorm_night.svg'));
    iconRegistry.addSvgIcon('thunderstorm', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_thunderstorm.svg'));
    // API liefert das hier nciht. warten, bis es mal verwendet wird.
    iconRegistry.addSvgIcon('rainheavy-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_rainheavy_night.svg'));
    iconRegistry.addSvgIcon('rainheavy', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_rainheavy.svg'));
    iconRegistry.addSvgIcon('showerslight-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_showers_night_light.svg'));
    iconRegistry.addSvgIcon('showerslight', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_showers_light.svg'));
    iconRegistry.addSvgIcon('showers-night', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_showers_night.svg'));
    iconRegistry.addSvgIcon('showers', sanitizer.bypassSecurityTrustResourceUrl('../assets/img/sym_showers.svg'));
   }

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
  public getCurrentConditions(): Promise<WeatherConditions> {
    return this
      ._httpClient
      .get(environment.apiBaseUrl + '/weather')
      .pipe(map((p: WeatherConditions) => p))
      .toPromise<WeatherConditions>();
  }

  public getIcon(sunshine: number, isNight: boolean) {
    let result = '';
    switch (sunshine) {
      case 0: result = 'sunshine'; break;
      case 1: result = 'overcast'; break;
      case 2: result = 'cloudy'; break;
      case 3: result = 'partlycloudy'; break;
      case 4: result = 'rain'; break;
      case 5: result = 'thunderstorm'; break;
    }
    if (isNight) {
      return result + '-night';
    }
    return result;
  }
}
