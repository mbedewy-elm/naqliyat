import type { BidDto, ConfirmArrivalDto, CreateBidDto, CreatePaymentDto, CreateTripDto, PaymentDto, RateTripDto, TripDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TripService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  approveBid = (bidId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BidDto>({
      method: 'POST',
      url: `/api/app/trip/approve-bid/${bidId}`,
    },
    { apiName: this.apiName,...config });
  

  confirmArrival = (tripId: string, input: ConfirmArrivalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto>({
      method: 'POST',
      url: `/api/app/trip/confirm-arrival/${tripId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateTripDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto>({
      method: 'POST',
      url: '/api/app/trip',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  createPayment = (tripId: string, input: CreatePaymentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaymentDto>({
      method: 'POST',
      url: `/api/app/trip/payment/${tripId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getBidById = (bidId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BidDto>({
      method: 'GET',
      url: `/api/app/trip/bid-by-id/${bidId}`,
    },
    { apiName: this.apiName,...config });
  

  getById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto>({
      method: 'GET',
      url: `/api/app/trip/${id}/by-id`,
    },
    { apiName: this.apiName,...config });
  

  getDriverTripById = (tripId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto>({
      method: 'GET',
      url: `/api/app/trip/driver-trip-by-id/${tripId}`,
    },
    { apiName: this.apiName,...config });
  

  getDriverTrips = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto[]>({
      method: 'GET',
      url: '/api/app/trip/driver-trips',
    },
    { apiName: this.apiName,...config });
  

  getNewBids = (tripId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BidDto[]>({
      method: 'GET',
      url: `/api/app/trip/new-bids/${tripId}`,
    },
    { apiName: this.apiName,...config });
  

  getOpenTrips = (locationFilter?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto[]>({
      method: 'GET',
      url: '/api/app/trip/open-trips',
      params: { locationFilter },
    },
    { apiName: this.apiName,...config });
  

  placeBid = (tripId: string, input: CreateBidDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BidDto>({
      method: 'POST',
      url: `/api/app/trip/place-bid/${tripId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  rateTrip = (tripId: string, input: RateTripDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TripDto>({
      method: 'POST',
      url: `/api/app/trip/rate-trip/${tripId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  rejectBid = (bidId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BidDto>({
      method: 'POST',
      url: `/api/app/trip/reject-bid/${bidId}`,
    },
    { apiName: this.apiName,...config });
}