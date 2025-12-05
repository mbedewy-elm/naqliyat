import { mapEnumToOptions } from '@abp/ng.core';

export enum TripStatuses {
  OpenForBidding = 1,
  WaitingBidAgreement = 2,
  WaitingPayment = 3,
  OnItsWay = 4,
  Arrived = 5,
}

export const tripStatusesOptions = mapEnumToOptions(TripStatuses);
