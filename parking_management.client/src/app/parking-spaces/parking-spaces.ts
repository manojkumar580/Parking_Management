import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';
import { ParkingSpace } from '../../models/parking-space.model';
import { BookingService } from '../../service/booking.service';
import { ParkingSpaceService } from '../../service/parking-space.service';
import { SubscriptionService } from '../../service/subscription.service';


@Component({
  selector: 'app-parking-spaces',
  templateUrl: './parking-spaces.component.html',
  styleUrl: './parking-spaces.component.css',
  standalone: false
})
export class ParkingSpacesComponent implements OnInit {

  parkingSpaces: ParkingSpace[] = [];

  isLoadingParkingSpaces = false;

  parkingSpaceError = '';

  bookingError = '';

  bookingSuccess = '';

  subscriptionError = '';

  subscriptionSuccess = '';

  // Tracks only the parking space currently being booked
  bookingInProgressId: string | null = null;

  // Tracks only the parking space currently being subscribed
  subscriptionInProgressId: string | null = null;


  constructor(
    private parkingSpaceService: ParkingSpaceService,
    private bookingService: BookingService,
    private subscriptionService: SubscriptionService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }


  ngOnInit(): void {

    this.loadParkingSpaces();

  }


  loadParkingSpaces(): void {

    this.isLoadingParkingSpaces = true;

    this.parkingSpaceError = '';

    this.parkingSpaceService
      .getAll()
      .subscribe({

        next: (spaces) => {

          console.log(
            'Parking spaces received:',
            spaces
          );

          this.parkingSpaces = spaces;

          this.isLoadingParkingSpaces = false;

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Parking spaces API error:',
            error
          );

          this.parkingSpaceError =
            error?.error?.message ??
            'Unable to load parking spaces.';

          this.isLoadingParkingSpaces = false;

          this.changeDetectorRef.detectChanges();
        }

      });

  }

  refreshParkingSpaces(): void {

    this.parkingSpaceService
      .getAll()
      .subscribe({

        next: (spaces) => {

          this.parkingSpaces = spaces;

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Unable to refresh parking spaces:',
            error
          );

        }

      });
  }

  bookParkingSpace(spaceId: string): void {

    this.bookingError = '';

    this.bookingSuccess = '';

    if (this.bookingInProgressId) {
      return;
    }

    this.bookingInProgressId = spaceId;

    const request = {
      parkingSpaceId: spaceId
    };


    this.bookingService
      .createBooking(request)
      .subscribe({

        next: (booking) => {

          console.log(
            'Booking created:',
            booking
          );

          this.bookingInProgressId = null;

          this.bookingSuccess =
            `Parking space ${booking.spaceNumber} booked successfully.`;

          this.refreshParkingSpaces();

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Booking error:',
            error
          );

          this.bookingInProgressId = null;

          this.bookingError =
            error?.error?.message ??
            'Unable to create booking.';

          this.changeDetectorRef.detectChanges();
        }

      });

  }


  subscribeParkingSpace(spaceId: string): void {

    this.subscriptionError = '';

    this.subscriptionSuccess = '';

    if (this.subscriptionInProgressId) {
      return;
    }

    this.subscriptionInProgressId = spaceId;

    const request = {
      parkingSpaceId: spaceId
    };


    this.subscriptionService
      .createSubscription(request)
      .subscribe({

        next: (subscription) => {

          console.log(
            'Subscription created:',
            subscription
          );

          this.subscriptionInProgressId = null;

          this.subscriptionSuccess =
            `Parking space ${subscription.spaceNumber} subscribed successfully.`;

          this.refreshParkingSpaces();
          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Subscription error:',
            error
          );

          this.subscriptionInProgressId = null;

          this.subscriptionError =
            error?.error?.message ??
            'Unable to create subscription.';

          this.changeDetectorRef.detectChanges();
        }

      });

  }

}
