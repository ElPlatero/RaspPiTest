import { Thermostat } from 'src/shared/classes/thermostat';
import { DeviceTemperature } from 'src/shared/classes/device-temperature';

export class Device {
    constructor() {
        this.deviceTemperature = new DeviceTemperature();
        this.thermostat = new Thermostat();
    }

    public name: string;
    public ain: string;
    public deviceTemperature: DeviceTemperature;
    public thermostat: Thermostat;
    public manufacturer: string;
    public productName: string;

    public assign(other: Device): Device {
        this.ain = other.ain;
        this.name = other.name;
        this.deviceTemperature = this.deviceTemperature || new DeviceTemperature();
        this.deviceTemperature.temperature = other.deviceTemperature.temperature;
        this.deviceTemperature.offset = other.deviceTemperature.offset;
        this.thermostat.assign(other.thermostat);
        this.manufacturer = other.manufacturer;
        this.productName = other.productName;

        return this;
    }
}
