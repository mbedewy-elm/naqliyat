import type { EntityDto } from '@abp/ng.core';
import type { BidStatus } from '../enums/bid-status.enum';
import type { TruckTypes } from '../enums/truck-types.enum';
import type { PaymentStatus } from '../enums/payment-status.enum';
import type { TripStatuses } from '../enums/trip-statuses.enum';

export interface BidDto extends EntityDto<string> {
  tripId?: string;
  truckId?: string;
  price: number;
  arrivalDate?: string;
  statusId?: BidStatus;
}

export interface ConfirmArrivalDto {
  otp?: string;
}

export interface CreateBidDto {
  truckId?: string;
  price: number;
  arrivalDate?: string;
}

export interface CreatePaymentDto {
  priceWithoutVat: number;
}

export interface CreateTripDto {
  fromLocation?: string;
  toLocation?: string;
  startDate?: string;
  endDate?: string;
  goodsWeight?: number;
  goodsDimensions?: string;
  truckTypeId?: TruckTypes;
  goodsType?: string;
  notes?: string;
  pictureIds: string[];
}

export interface PaymentDto extends EntityDto<string> {
  tripId?: string;
  priceWithoutVat: number;
  referenceId?: string;
  expiryDate?: string;
  status?: PaymentStatus;
}

export interface RateTripDto {
  rate: number;
  note?: string;
}

export interface TripDto extends EntityDto<string> {
  fromLocation?: string;
  toLocation?: string;
  startDate?: string;
  endDate?: string;
  goodsWeight?: number;
  goodsDimensions?: string;
  truckTypeId?: TruckTypes;
  goodsType?: string;
  notes?: string;
  statusId?: TripStatuses;
  pictureIds: string[];
}
