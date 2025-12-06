import type { TruckTypes } from '../enums/truck-types.enum';
import type { TemperatureRequirements } from '../enums/temperature-requirements.enum';
import type { EntityDto } from '@abp/ng.core';

export interface CreateDriverDto {
  name?: string;
  phone?: string;
  identification?: string;
  licenseNumber?: string;
  photoId?: string;
  nationalityId: number;
  userId?: string;
}

export interface CreateTruckDto {
  make?: string;
  model?: string;
  year?: string;
  licenseNumber?: string;
  registrationNumber?: string;
  truckTypeId?: TruckTypes;
  temperatureRequirement?: TemperatureRequirements;
  ownerId?: string;
  driverId?: string;
  maxLoad: number;
  pictureIds: string[];
}

export interface DriverDto extends EntityDto<string> {
  name?: string;
  phone?: string;
  identification?: string;
  licenseNumber?: string;
  photoId?: string;
  nationalityId: number;
  userId?: string;
}

export interface TruckDto extends EntityDto<string> {
  make?: string;
  model?: string;
  year?: string;
  licenseNumber?: string;
  registrationNumber?: string;
  truckTypeId?: TruckTypes;
  temperatureRequirement?: TemperatureRequirements;
  ownerId?: string;
  driverId?: string;
  maxLoad: number;
  pictureIds: string[];
}
