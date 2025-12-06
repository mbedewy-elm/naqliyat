import { mapEnumToOptions } from '@abp/ng.core';

export enum TemperatureRequirements {
  Ambient = 0,
  Chilled = 1,
  Frozen = 2,
  DeepFrozen = 3,
}

export const temperatureRequirementsOptions = mapEnumToOptions(TemperatureRequirements);
