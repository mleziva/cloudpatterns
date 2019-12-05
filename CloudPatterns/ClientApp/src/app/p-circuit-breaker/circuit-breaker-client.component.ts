import { Component, Inject } from '@angular/core';
import { HttpClient} from '@angular/common/http';

@Component({
    selector: 'circuit-breaker-client',
    template: `<div class="card">
                <div class="card-header">
                  Client Simulator
                </div>
                <div class="card-body">
                  <div class ="row">
                  <div class ="col-sm-8">
<button type="button" class="btn btn-primary" aria-pressed="false" autocomplete="off" (click)="performClientRequest()">
                    Execute request
                  </button>
</div>
                  <div class ="col-sm-4">

      <div class="progress-loader" [hidden]="!loading">
                      <mat-progress-spinner [mode]="'indeterminate'" [diameter]="20">
                      </mat-progress-spinner>
                  </div>
</div>
                  </div>
                  

                  
            
                </div>
                <ul class="list-group list-group-flush">
                  <li class="list-group-item">Executed Time: {{cbResponse?.executedTime | date : 'mediumTime'}} </li>
                  <li class="list-group-item">Response Time: {{cbResponse?.responseTime}} ms </li>
                  <li class="list-group-item">Status: {{cbResponse?.isSuccess ? "Success" : "Fail"}}</li>
                  <li class="list-group-item">Message: {{cbResponse?.message}}</li>
                </ul>
              </div>`
})
export class CircuitBreakerClientComponent {

    private cbServiceUrl = "api/circuitbreaker";
    public cbResponse: CBServiceResponse;
    private loading: boolean = false;
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    performClientRequest() {
        this.loading = true;
        let executedTime = new Date();
        this.http.get<CBServiceResponse>(this.baseUrl + this.cbServiceUrl).subscribe(result => {
            this.cbResponse = result
            this.cbResponse.executedTime = executedTime;
            this.cbResponse.responseTime = new Date().getTime() - executedTime.getTime();
            this.loading = false;
        }, error => console.error(error));
    }
}
interface CBServiceResponse {
    executedTime: Date;
    responseTime: number;
    isSuccess: boolean;
    message: string;
}
