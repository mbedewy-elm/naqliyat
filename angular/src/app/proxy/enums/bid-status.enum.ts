import { mapEnumToOptions } from '@abp/ng.core';

export enum BidStatus {
  New = 1,
  Accepted = 2,
  NotChoosen = 3,
  Rejected = 4,
}

export const bidStatusOptions = mapEnumToOptions(BidStatus);
