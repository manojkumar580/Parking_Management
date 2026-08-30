export interface CreateBookingRequest {
  parkingSpaceId: string;
}

export interface Booking {
  id: string;
  parkingSpaceId: string;
  spaceNumber: string;
  checkInTime: string;
  checkOutTime: string | null;
  amount: number | null;
  status: string;
  createdAt: string;
}
