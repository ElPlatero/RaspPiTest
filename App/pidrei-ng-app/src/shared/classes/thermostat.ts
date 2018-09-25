import { ThermostatChange } from './thermostat-change';

export class Thermostat {
    constructor() {
        this.nextChange = new ThermostatChange();
    }
    public actualTemperature: number;
    public nominalTemperature: number;
    public savingTemperature: number;
    public comfortTemperature: number;
    public isUiLocked: Boolean;
    public isDeviceLocked: Boolean;
    public errorCode: number;
    public isBatteryLow: Boolean;
    public nextChange: ThermostatChange;

    public assign(other: Thermostat): Thermostat {
        this.actualTemperature = other.actualTemperature;
        this.nominalTemperature = other.nominalTemperature;
        this.savingTemperature = other.savingTemperature;
        this.comfortTemperature = other.comfortTemperature;
        this.isUiLocked = other.isUiLocked;
        this.isDeviceLocked = other.isDeviceLocked;
        this.errorCode = other.errorCode;
        this.isBatteryLow = other.isBatteryLow;
        this.nextChange.changeTime = other.nextChange.changeTime;
        this.nextChange.newTemperature = other.nextChange.newTemperature;

        return this;
    }

    public isActivated(): boolean {
        return this.nominalTemperature !== -273.15;
    }
}
