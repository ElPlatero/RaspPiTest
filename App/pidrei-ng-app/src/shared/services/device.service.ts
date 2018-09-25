import { Injectable } from '@angular/core';
import { HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Device } from '../classes/device';
import { DeviceListResponse } from '../classes/device-list-response';
import { environment } from '../../environments/environment';
import { DeviceChangeStatusResponse } from '../classes/device-change-status-response';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {

  constructor(private http: HttpClient) { }

  private getOptions() {
    const headers = new HttpHeaders().append('Content-Type', 'application/json');
    return { headers: headers };
  }


  public getDevices(): Observable<Device[]> {
    return this.http.get(environment.apiBaseUrl + '/device')
      .pipe(map((response: DeviceListResponse) => {
        const devices = response.devices;
        return devices.map(device => new Device().assign(device));
      }))
      .pipe(catchError(this.handleError('get-devices', []))
    );
  }

  public changeStatus(device: Device): Observable<Device> {
    return this.setTemperature(device, device.thermostat.isActivated() ? -273.15 : device.thermostat.actualTemperature);
  }

  public setTemperature(device: Device, value: number): Observable<Device> {
    const uri = environment.apiBaseUrl + '/device/' + encodeURI(device.ain) + '/status';
    return this.http.put(uri, value, this.getOptions())
      .pipe(map((response: DeviceChangeStatusResponse) => {
        if (!response.success) {
          return device;
        }
        device.thermostat.nominalTemperature = response.result;
        return device;
      }))
      .pipe(catchError(this.handleError('change-device-temperature', device)));
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
         console.error(error);
         return of(result as T);
       };
  }
}
