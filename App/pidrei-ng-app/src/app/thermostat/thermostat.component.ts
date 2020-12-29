import { Component, OnInit, Input } from '@angular/core';
import { DeviceService } from 'src/shared/services/device.service';
import { Device } from 'src/shared/classes/device';
import { formatNumber } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
@Component({
  selector: 'app-thermostat',
  templateUrl: './thermostat.component.html',
  styleUrls: ['./thermostat.component.css']
})
export class ThermostatComponent implements OnInit {
  @Input()
  public device: Device;
  public isOn: boolean;
  public nominalTemperature: BehaviorSubject<number>;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    if (this.device) {
      this.isOn = this.device.thermostat.isActivated();
      this.nominalTemperature = new BehaviorSubject<number>(this.device.thermostat.nominalTemperature);
      this.nominalTemperature.subscribe((value: number) => {
        if (this.getStatus()) {
          if (value !== this.device.thermostat.nominalTemperature) {
            this.setNominalTemperature(value);
          }
        }
      });
    }
  }

  getStatus(): boolean {
    if (!this.device) {
      return false;
    }

    if (!this.device.thermostat) {
      return false;
    }

    return this.device.thermostat.nominalTemperature !== -273.15;
  }

  getNominalTemperatureText(): string {
    if (this.getStatus()) {
      return this.device.thermostat.nominalTemperature === 0
      ? 'AN'
      : formatNumber(this.device.thermostat.nominalTemperature, 'en-EN', '2.1-2') + '°C';
    }
    return 'AUS';
  }

  getNextScheduledTemperatureChange() {
    return this.device.thermostat.nextChange.newTemperature === 0 ? 'AUS' : formatNumber(this.device.thermostat.nextChange.newTemperature, 'en-EN', '2.1-2') + '°C';
  }

  onButtonChanged(newStatus: boolean): void {
    this.deviceService.changeStatus(this.device).subscribe(p => {
      this.device.assign(p);
      this.nominalTemperature.next(this.device.thermostat.nominalTemperature);
    });
  }

  setNominalTemperature(value: number): void {
    this.deviceService.setTemperature(this.device, value).subscribe((device: Device) => {
      this.device.thermostat.nominalTemperature = device.thermostat.nominalTemperature;
    });
  }
}
