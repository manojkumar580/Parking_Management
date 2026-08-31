import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { LoginComponent } from './login.component/login.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { authInterceptor } from './interceptors/auth.interceptor';
import { RegisterComponent } from './register.component/register.component';
import { DashboardComponent } from './dashboard.component/dashboard.component';
import { ParkingSpacesComponent } from './parking-spaces/parking-spaces';
import { SubscriptionsComponent } from './subscriptions/subscriptions';
import { BookingsComponent } from './bookings/bookings';
import { ManageBookingsComponent } from './admin/manage-bookings.component/manage-bookings.component';
import { ManageSubscriptionsComponent } from './admin/manage-subscriptions.component/manage-subscriptions.component';
import { ManageUsersComponent } from './admin/manage-users.component/manage-users.component';
import { Admin } from './admin/admin';

@NgModule({
  declarations: [
    App,
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    ParkingSpacesComponent,
    BookingsComponent,
    SubscriptionsComponent,
    ManageBookingsComponent,
    ManageSubscriptionsComponent,
    ManageUsersComponent,
    Admin
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    CommonModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([authInterceptor]))
  ],
  bootstrap: [App]
})
export class AppModule { }
