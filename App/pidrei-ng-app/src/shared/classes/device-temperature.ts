export class DeviceTemperature {
    public temperature: number;
    public offset: number;

    public getTemperature(): number {
        return this.temperature + this.offset;
    }
}
