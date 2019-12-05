import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CircuitBreakerComponent } from './p-circuit-breaker/circuit-breaker.component';
import { CircuitBreakerClientComponent } from './p-circuit-breaker/circuit-breaker-client.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SidebarMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
        CircuitBreakerComponent,
        CircuitBreakerClientComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
        { path: 'circuit-breaker', component: CircuitBreakerComponent },
    ]),
      BrowserAnimationsModule,
      MatProgressSpinnerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
