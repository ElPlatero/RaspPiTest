import { WindInformation } from './wind-information';

export class DayForecast {
    public date: Date;
    public morning: number;
    public afternoon: number;
    public night: number;
    public maxTemperature: number;
    public minTemperature: number;
    public wind: WindInformation;
    public expectedRain: number;
}
