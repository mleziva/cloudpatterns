import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-circuit-breaker',
  templateUrl: './circuit-breaker.component.html'
})
export class CircuitBreakerComponent {
    private stateUrl = "api/circuitbreaker/state";
    private manageServiceUrl = "api/CircuitBreaker/manage/Service";
    private manageCBUrl = "api/CircuitBreaker/manage/circuitbreaker";
    public cbState: CircuitBreakerState;

    private headers = new HttpHeaders().set('Content-Type', 'application/+json');


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
      this.refreshState();
    }

    refreshState() {
        this.http.get<CircuitBreakerState>(this.baseUrl + this.stateUrl).subscribe(result => {
            this.cbState = result;
            this.cbState.lastRefreshed = new Date();
            console.log(result);
        }, error => console.error(error));
    }
    setServiceState() {
        let serviceEnabled = !this.cbState.serviceEnabled;
        this.http.put<boolean>(this.baseUrl + this.manageServiceUrl, serviceEnabled, {headers: this.headers}).subscribe(result => {
            this.cbState.serviceEnabled = serviceEnabled;
        }, error => console.error(error));
    }
    setCBState() {
        let cbEnabled = !this.cbState.circuitBreakerEnabled;
        this.http.put<boolean>(this.baseUrl + this.manageCBUrl, cbEnabled, { headers: this.headers }).subscribe(result => {
            this.cbState.circuitBreakerEnabled = cbEnabled;
        }, error => console.error(error));
    }

}

interface CircuitBreakerState {
    serviceEnabled: boolean;
    circuitBreakerEnabled: boolean;
    circuitBreakerState: string;
    lastRefreshed: Date;
}



