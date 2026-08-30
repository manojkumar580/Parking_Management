import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';
import { Subscription } from '../../models/subscription.model';
import { SubscriptionService } from '../../service/subscription.service';


@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.css',
  standalone: false
})
export class SubscriptionsComponent implements OnInit {

  subscriptions: Subscription[] = [];

  isLoadingSubscriptions = false;

  subscriptionError = '';

  cancelSuccess = '';

  cancelInProgressId: string | null = null;


  constructor(
    private subscriptionService: SubscriptionService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }


  ngOnInit(): void {

    this.loadSubscriptions();

  }


  loadSubscriptions(): void {

    this.isLoadingSubscriptions = true;

    this.subscriptionError = '';

    this.subscriptionService
      .getMySubscriptions()
      .subscribe({

        next: (subscriptions) => {

          console.log(
            'Subscriptions received:',
            subscriptions
          );

          this.subscriptions = subscriptions;

          this.isLoadingSubscriptions = false;

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Subscriptions API error:',
            error
          );

          this.subscriptionError =
            error?.error?.message ??
            'Unable to load your subscriptions.';

          this.isLoadingSubscriptions = false;

          this.changeDetectorRef.detectChanges();
        }

      });

  }


  cancelSubscription(
    subscriptionId: string
  ): void {

    if (this.cancelInProgressId) {
      return;
    }

    this.subscriptionError = '';

    this.cancelSuccess = '';

    this.cancelInProgressId = subscriptionId;


    this.subscriptionService
      .cancelSubscription(subscriptionId)
      .subscribe({

        next: (subscription) => {

          console.log(
            'Subscription cancelled:',
            subscription
          );

          this.cancelInProgressId = null;

          this.cancelSuccess =
            'Subscription cancelled successfully.';

          this.loadSubscriptions();

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Subscription cancellation error:',
            error
          );

          this.cancelInProgressId = null;

          this.subscriptionError =
            error?.error?.message ??
            'Unable to cancel subscription.';

          this.changeDetectorRef.detectChanges();
        }

      });

  }

}
