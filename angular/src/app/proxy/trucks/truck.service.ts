import type { CreateDriverDto, CreateTruckDto, DriverDto, TruckDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TruckService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  create = (input: CreateTruckDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TruckDto>({
      method: 'POST',
      url: '/api/app/truck',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  createDriver = (input: CreateDriverDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DriverDto>({
      method: 'POST',
      url: '/api/app/truck/driver',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  getDriverById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DriverDto>({
      method: 'GET',
      url: `/api/app/truck/${id}/driver-by-id`,
    },
    { apiName: this.apiName,...config });
  

  getDriverByUserId = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DriverDto>({
      method: 'GET',
      url: `/api/app/truck/driver-by-user-id/${userId}`,
    },
    { apiName: this.apiName,...config });
  

  getDriverList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DriverDto[]>({
      method: 'GET',
      url: '/api/app/truck/driver-list',
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, TruckDto[]>({
      method: 'GET',
      url: '/api/app/truck',
    },
    { apiName: this.apiName,...config });
  

  getTruckByDriverId = (driverId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TruckDto>({
      method: 'GET',
      url: `/api/app/truck/truck-by-driver-id/${driverId}`,
    },
    { apiName: this.apiName,...config });
}