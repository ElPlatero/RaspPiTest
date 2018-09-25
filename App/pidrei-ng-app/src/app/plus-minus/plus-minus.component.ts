import { Component, OnInit, Input } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-plus-minus',
  templateUrl: './plus-minus.component.html',
  styleUrls: ['./plus-minus.component.css']
})
export class PlusMinusComponent implements OnInit {
  @Input()
  public temperature:  BehaviorSubject<number>;

  constructor() { }

  ngOnInit() {
  }

  public onDecrease(evt: Event): void {
    if (this.temperature.getValue() === 16.0) {
      return;
    }

    this.temperature.next(this.temperature.getValue() - 0.5);
  }

  public onIncrease(evt: Event): void {
    if (this.temperature.getValue() === 28.0) {
      return;
    }

    this.temperature.next(this.temperature.getValue() + 0.5);
  }
}
