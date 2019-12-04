import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-circuit-breaker',
  templateUrl: './circuit-breaker.component.html'
})
export class CircuitBreakerComponent {
    public cbstate: CircuitBreakerState = {circuitBreakerClosed: false, circuitBreakerEnabled: false, serviceEnabled: false};

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      http.get<CircuitBreakerState>(baseUrl + 'api/circuitbreaker/state').subscribe(result => {
          this.cbstate = result;
    }, error => console.error(error));
  }
}

interface CircuitBreakerState {
    serviceEnabled: boolean;
    circuitBreakerEnabled: boolean;
    circuitBreakerClosed: boolean;
}
