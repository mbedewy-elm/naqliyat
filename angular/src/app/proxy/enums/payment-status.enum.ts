import { mapEnumToOptions } from '@abp/ng.core';

export enum PaymentStatus {
  Pending = 1,
  Paid = 2,
  Cancelled = 3,
  Expired = 4,
}

export const paymentStatusOptions = mapEnumToOptions(PaymentStatus);
