import type { RegisterAsDriverDto, RegisterAsOwnerDto, UserRegistrationResultDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private restService = inject(RestService);
  apiName = 'Default';
  

  registerAsDriver = (input: RegisterAsDriverDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserRegistrationResultDto>({
      method: 'POST',
      url: '/api/app/user/register-as-driver',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  registerAsOwner = (input: RegisterAsOwnerDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserRegistrationResultDto>({
      method: 'POST',
      url: '/api/app/user/register-as-owner',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  registerAsRequester = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserRegistrationResultDto>({
      method: 'POST',
      url: '/api/app/user/register-as-requester',
    },
    { apiName: this.apiName,...config });
}