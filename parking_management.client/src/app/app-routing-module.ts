import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login.component/login.component';
import { RegisterComponent } from './register.component/register.component';
import { DashboardComponent } from './dashboard.component/dashboard.component';
import { ParkingSpacesComponent } from './parking-spaces/parking-spaces';
import { BookingsComponent } from './bookings/bookings';
import { SubscriptionsComponent } from './subscriptions/subscriptions';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    children: [

      {
        path: '',
        redirectTo: 'parking',
        pathMatch: 'full'
      },

      {
        path: 'parking',
        component: ParkingSpacesComponent
      },

      {
        path: 'bookings',
        component: BookingsComponent
      },

      {
        path: 'subscriptions',
        component: SubscriptionsComponent
      }

    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard/parking'
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
