import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HttpClient } from '@angular/common/http';
import { DeviceService } from '../shared/services/device.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { FritzDevicesComponent} from './fritz-devices/fritz-devices.component';
import { ThermostatComponent } from './thermostat/thermostat.component';
import {
  MatButtonModule,
  MatCardModule,
  MatIconModule,
  MatMenuModule,
  MatSlideToggleModule,
  MatToolbarModule,
  MatListModule,
  MatSidenavModule
} from '@angular/material';
import { PlusMinusComponent } from './plus-minus/plus-minus.component';
import { AppRoutingModule, routes } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { WeatherComponent } from './weather/weather.component';
import { LightsComponent } from './lights/lights.component';
import { DayForecastComponent } from './weather/day-forecast/day-forecast.component';

@NgModule({
  declarations: [
    AppComponent, FritzDevicesComponent, ThermostatComponent, PlusMinusComponent, WeatherComponent, LightsComponent, DayForecastComponent
  ],

  imports: [
    RouterModule.forRoot(routes),
    BrowserModule, BrowserAnimationsModule, HttpClientModule, FormsModule, MatSlideToggleModule, MatCardModule, MatButtonModule,
    MatMenuModule, MatIconModule, MatToolbarModule, MatListModule, MatSidenavModule, AppRoutingModule
  ],

  providers: [HttpClient, DeviceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
