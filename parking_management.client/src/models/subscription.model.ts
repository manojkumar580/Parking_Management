export interface Subscription {
  id: string;
  parkingSpaceId: string;
  spaceNumber: string;
  startDate: string;
  endDate: string;
  amount: number;
  status: string;
  createdAt: string;
}

export interface CreateSubscriptionRequest {
  parkingSpaceId: string;
}
