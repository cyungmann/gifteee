import { Component, effect, signal } from '@angular/core';
import { WeatherForecast } from './weather-forecast';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [AsyncPipe],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  forecasts$: Observable<ReadonlyArray<Readonly<WeatherForecast>>> = null!;

  constructor(private readonly http: HttpClient) {
    effect(() => {
      this.forecasts$ = this.http.get<WeatherForecast[]>('/api/weatherforecast');
    })
  }

  protected readonly title = signal('gifteee');
}
