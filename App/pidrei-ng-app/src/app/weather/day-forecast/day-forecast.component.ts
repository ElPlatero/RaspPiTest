import { Component, OnInit, Input } from '@angular/core';
import { DayForecast } from '../../../shared/classes/day-forecast';
import { DomSanitizer } from '@angular/platform-browser';
import { MatIconRegistry } from '@angular/material';
import { WindInformation } from '../../../shared/classes/wind-information';

@Component({
  selector: 'app-day-forecast',
  templateUrl: './day-forecast.component.html',
  styleUrls: ['./day-forecast.component.css']
})
export class DayForecastComponent implements OnInit {
  @Input() public forecast: DayForecast;

  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
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
}
