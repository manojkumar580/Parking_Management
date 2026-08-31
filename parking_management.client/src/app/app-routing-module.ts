import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login.component/login.component';
import { RegisterComponent } from './register.component/register.component';
import { DashboardComponent } from './dashboard.component/dashboard.component';
import { ParkingSpacesComponent } from './parking-spaces/parking-spaces';
import { BookingsComponent } from './bookings/bookings';
import { SubscriptionsComponent } from './subscriptions/subscriptions';
import { Admin } from './admin/admin';
import { ManageBookingsComponent } from './admin/manage-bookings.component/manage-bookings.component';
import { ManageSubscriptionsComponent } from './admin/manage-subscriptions.component/manage-subscriptions.component';
import { ManageUsersComponent } from './admin/manage-users.component/manage-users.component';

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
    path: 'admin',
    component: Admin,
    children: [
      {
        path: '',
        redirectTo: 'manage-bookings',
        pathMatch:'full'
      },
      {
        path: 'manage-bookings',
        component:ManageBookingsComponent
      },
      {
        path: 'manage-subscription',
        component:ManageSubscriptionsComponent
      },
      {
        path: 'manage-users',
        component:ManageUsersComponent
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
