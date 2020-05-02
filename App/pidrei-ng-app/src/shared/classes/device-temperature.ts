export class DeviceTemperature {
    public temperature: number;
    public offset: number;

    constructor(initialValue: { temperature?: number, offset?: number } = { temperature: 0, offset: 0 }) {
        this.temperature = initialValue.temperature;
        this.offset = initialValue.offset;
    }

    public getTemperature(): number {
        return this.temperature + this.offset;
    }
}
