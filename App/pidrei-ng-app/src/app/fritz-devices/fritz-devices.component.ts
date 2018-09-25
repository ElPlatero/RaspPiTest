import { Component, OnInit } from '@angular/core';
import { Device } from '../../shared/classes/device';
import { DeviceService } from '../../shared/services/device.service';
import { timer } from 'rxjs';

@Component({
  selector: 'app-fritz-devices',
  templateUrl: './fritz-devices.component.html',
  styleUrls: ['./fritz-devices.component.css']
})
export class FritzDevicesComponent implements OnInit {
  public title = 'Heizungen';
  public deviceList: Device[];
  private _interval = 60000;

  constructor(private deviceService: DeviceService ) { }

  ngOnInit() {
    this.deviceService.getDevices().subscribe((p: Device[]) => this.deviceList = p);
    timer(this._interval, this._interval).subscribe((p: number) => {
      this.deviceService.getDevices().subscribe((devices: Device[]) => {
        for (const device of devices) {
          this.deviceList.find(q => q.ain === device.ain).assign(device);
        }
      });
    });
  }

}
