
export interface RegisterAsDriverDto {
  name?: string;
  phone?: string;
  identification?: string;
  licenseNumber?: string;
  photoId?: string;
  nationalityId: number;
}

export interface RegisterAsOwnerDto {
  name?: string;
  phone?: string;
  identification?: string;
  entityNumber?: string;
  nationalityId: number;
}

export interface UserRegistrationResultDto {
  success: boolean;
  role?: string;
  entityId?: string;
  message?: string;
}
