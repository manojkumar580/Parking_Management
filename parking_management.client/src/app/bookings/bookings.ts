import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';
import { Booking } from '../../models/booking.model';
import { BookingService } from '../../service/booking.service';



@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css',
  standalone: false
})
export class BookingsComponent implements OnInit {

  bookings: Booking[] = [];

  isLoadingBookings = false;

  bookingListError = '';

  checkoutInProgressId: string | null = null;

  constructor(
    private bookingService: BookingService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }


  ngOnInit(): void {

    this.loadBookings();

  }


  loadBookings(): void {

    this.isLoadingBookings = true;

    this.bookingListError = '';

    this.bookingService
      .getMyBookings()
      .subscribe({

        next: (bookings) => {

          console.log(
            'Bookings received:',
            bookings
          );

          this.bookings = bookings;

          this.isLoadingBookings = false;

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Bookings API error:',
            error
          );

          this.bookingListError =
            error?.error?.message ??
            'Unable to load your bookings.';

          this.isLoadingBookings = false;

          this.changeDetectorRef.detectChanges();
        }

      });

  }


  checkoutBooking(bookingId: string): void {

    if (this.checkoutInProgressId) {
      return;
    }

    this.checkoutInProgressId = bookingId;

    this.bookingService
      .checkoutBooking(bookingId)
      .subscribe({

        next: (booking) => {

          console.log(
            'Checkout successful:',
            booking
          );

          this.checkoutInProgressId = null;

          this.loadBookings();

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Checkout error:',
            error
          );

          this.checkoutInProgressId = null;

          this.bookingListError =
            error?.error?.message ??
            'Unable to checkout booking.';

          this.changeDetectorRef.detectChanges();
        }

      });

  }

}
