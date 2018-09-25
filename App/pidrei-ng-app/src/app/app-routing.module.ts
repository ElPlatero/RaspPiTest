import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FritzDevicesComponent } from './fritz-devices/fritz-devices.component';
import { WeatherComponent } from './weather/weather.component';
import { LightsComponent } from './lights/lights.component';

export const routes = [
  { path: 'heating', component: FritzDevicesComponent },
  { path: 'weather', component: WeatherComponent },
  { path: 'light', component: LightsComponent},
  { path: '**', redirectTo: 'heating'},
];

@NgModule({
  exports: [ RouterModule ]
})
export class AppRoutingModule { }
