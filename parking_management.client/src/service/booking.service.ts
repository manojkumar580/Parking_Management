import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Booking,
  CreateBookingRequest
} from '../models/booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  private readonly apiUrl =
    'https://localhost:7295/api/Booking';

  constructor(private http: HttpClient) { }

  createBooking(
    request: CreateBookingRequest
  ): Observable<Booking> {
    return this.http.post<Booking>(
      this.apiUrl,
      request
    );
  }

  getMyBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(
      `${this.apiUrl}/my`
    );
  }

  checkoutBooking(
    bookingId: string
  ): Observable<Booking> {
    return this.http.post<Booking>(
      `${this.apiUrl}/${bookingId}/checkout`,
      {}
    );
  }
}
