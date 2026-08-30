import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Subscription,
  CreateSubscriptionRequest
} from '../models/subscription.model';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {

  private readonly apiUrl =
    'https://localhost:7295/api/Subscription';

  constructor(private http: HttpClient) {
  }

  createSubscription(
    request: CreateSubscriptionRequest
  ): Observable<Subscription> {

    return this.http.post<Subscription>(
      this.apiUrl,
      request
    );
  }

  getMySubscriptions(): Observable<Subscription[]> {

    return this.http.get<Subscription[]>(
      `${this.apiUrl}/my`
    );
  }

  cancelSubscription(
    subscriptionId: string
  ): Observable<Subscription> {

    return this.http.post<Subscription>(
      `${this.apiUrl}/${subscriptionId}/cancel`,
      {}
    );
  }
}
