import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ParkingSpace } from '../../models/parking-space.model';
import { ParkingSpaceService } from '../../service/parking-space.service';
import { BookingService } from '../../service/booking.service';
import { finalize } from 'rxjs';
import { Booking } from '../../models/booking.model';
import { Subscription } from '../../models/subscription.model';
import { SubscriptionService } from '../../service/subscription.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  standalone: false
})
export class DashboardComponent implements OnInit {

  userName = '';
  userEmail = '';

  parkingSpaces: ParkingSpace[] = [];
  isLoadingParkingSpaces = false;
  parkingSpaceError = '';
  bookingError = '';
  bookingSuccess = '';
  bookingInProgressSpaceId: string | null = null;
  myBookings: Booking[] = [];
  isLoadingBookings = false;
  bookingListError = '';

  subscriptions: Subscription[] = [];
  isLoadingSubscriptions = false;
  subscriptionError = '';
  subscriptionInProgressSpaceId: string | null = null;
  subscriptionSuccess = '';
  cancellingSubscriptionId: string | null = null;

  constructor(
    private router: Router,
    private parkingSpaceService: ParkingSpaceService,
    private bookingService: BookingService,
    private subscriptionService: SubscriptionService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    try {
      const userJson = localStorage.getItem('parking_user');

      if (!userJson) {
        this.router.navigate(['/login']);
        return;
      }

      const user = JSON.parse(userJson);

      this.userName = user.name ?? '';
      this.userEmail = user.email ?? '';

      this.loadParkingSpaces();
      this.loadMyBookings();
      this.loadMySubscriptions();

    } catch (error) {
      console.error(
        'Error loading dashboard:',
        error
      );

      localStorage.removeItem('parking_auth_token');
      localStorage.removeItem('parking_user');

      this.router.navigate(['/login']);
    }
  }

  cancelSubscription(subscriptionId: string): void {
    this.subscriptionError = '';
    this.subscriptionSuccess = '';

    if (this.cancellingSubscriptionId !== null) {
      return;
    }

    this.cancellingSubscriptionId = subscriptionId;

    this.subscriptionService.cancelSubscription(subscriptionId)
      .pipe(
        finalize(() => {
          this.cancellingSubscriptionId = null;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (subscription) => {
          console.log(
            'Subscription cancelled:',
            subscription
          );

          this.subscriptionSuccess =
            `Subscription for ${subscription.spaceNumber} cancelled successfully.`;

          this.loadMySubscriptions();
          this.loadParkingSpaces();

          this.cdr.detectChanges();
        },

        error: (error) => {
          console.error(
            'Cancel subscription API error:',
            error
          );

          this.subscriptionError =
            error?.error?.message ??
            error?.error?.detail ??
            'Unable to cancel subscription.';

          this.cdr.detectChanges();
        }
      });
  }

  createSubscription(spaceId: string): void {
    this.subscriptionError = '';
    this.subscriptionSuccess = '';

    if (this.subscriptionInProgressSpaceId !== null) {
      return;
    }

    this.subscriptionInProgressSpaceId = spaceId;

    const request = {
      parkingSpaceId: spaceId
    };

    this.subscriptionService.createSubscription(request)
      .pipe(
        finalize(() => {
          this.subscriptionInProgressSpaceId = null;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (subscription) => {
          console.log(
            'Subscription created:',
            subscription
          );

          this.subscriptionSuccess =
            `Parking space ${subscription.spaceNumber} subscribed successfully.`;

          this.loadMySubscriptions();
          this.loadParkingSpaces();

          this.cdr.detectChanges();
        },

        error: (error) => {
          console.error(
            'Subscription API error:',
            error
          );

          this.subscriptionError =
            error?.error?.message ??
            error?.error?.detail ??
            'Unable to create subscription.';

          this.cdr.detectChanges();
        }
      });
  }

  loadMySubscriptions(): void {
    this.isLoadingSubscriptions = true;
    this.subscriptionError = '';

    console.log('Calling my subscriptions API...');

    this.subscriptionService.getMySubscriptions()
      .pipe(
        finalize(() => {
          console.log(
            'My subscriptions API request completed'
          );

          this.isLoadingSubscriptions = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (subscriptions) => {
          console.log(
            'My subscriptions received:',
            subscriptions
          );

          this.subscriptions = subscriptions;

          this.cdr.detectChanges();
        },

        error: (error) => {
          console.error(
            'My subscriptions API error:',
            error
          );

          this.subscriptionError =
            error?.error?.message ??
            error?.error?.detail ??
            'Unable to load your subscriptions.';

          this.cdr.detectChanges();
        }
      });
  }

  loadParkingSpaces(): void {
    this.isLoadingParkingSpaces = true;
    this.parkingSpaceError = '';

    console.log('Calling parking spaces API...');

    this.parkingSpaceService.getAll()
      .pipe(
        finalize(() => {
          console.log('Parking spaces API request completed');

          this.isLoadingParkingSpaces = false;

          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (spaces) => {
          console.log('Parking spaces received:', spaces);

          this.parkingSpaces = spaces;

          this.cdr.detectChanges();
        },

        error: (error) => {
          console.error(
            'Parking spaces API error:',
            error
          );

          this.parkingSpaceError =
            error?.error?.message ??
            'Unable to load parking spaces.';

          this.cdr.detectChanges();
        }
      });
  }

  logout(): void {
    try {
      localStorage.removeItem('parking_auth_token');
      localStorage.removeItem('parking_user');

      this.router.navigate(['/login']);
    } catch (error) {
      console.error(
        'Error during logout:',
        error
      );
    }
  }

  bookParkingSpace(spaceId: string): void {
    this.bookingError = '';
    this.bookingSuccess = '';

    if (this.bookingInProgressSpaceId !== null) {
      return;
    }

    this.bookingInProgressSpaceId = spaceId;

    const request = {
      parkingSpaceId: spaceId
    };

    this.bookingService.createBooking(request)
      .pipe(
        finalize(() => {
          // Always reset the loading state
          // success OR error
          this.bookingInProgressSpaceId = null;
        })
      )
      .subscribe({
        next: (booking) => {
          console.log('Booking successful:', booking);

          this.bookingSuccess =
            `Parking space ${booking.spaceNumber} booked successfully.`;

          this.loadParkingSpaces();
          this.loadMyBookings();
        },

        error: (error) => {
          console.error('Booking API error:', error);
          console.error('Status:', error.status);
          console.error('Response:', error.error);

          this.bookingError =
            error?.error?.message ??
            error?.error?.detail ??
            'Unable to create booking.';
          this.cdr.detectChanges();
        }
      });
  }

  loadMyBookings(): void {
    this.isLoadingBookings = true;
    this.bookingListError = '';

    this.bookingService.getMyBookings()
      .pipe(
        finalize(() => {
          this.isLoadingBookings = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (bookings) => {
          console.log('My bookings received:', bookings);

          this.myBookings = bookings;
        },

        error: (error) => {
          console.error(
            'Error loading bookings:',
            error
          );

          this.bookingListError =
            error?.error?.message ??
            'Unable to load your bookings.';
        }
      });
  }
}
