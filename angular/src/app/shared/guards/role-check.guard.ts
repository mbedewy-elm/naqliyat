import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService, ConfigStateService } from '@abp/ng.core';
import { map, take } from 'rxjs/operators';

export const roleCheckGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const configState = inject(ConfigStateService);
  const router = inject(Router);

  // If not authenticated, let the auth guard handle it
  if (!authService.isAuthenticated) {
    return true;
  }

  // Check if user has any roles
  const currentUser = configState.getOne('currentUser');
  const roles = currentUser?.roles || [];

  // If user has no roles, redirect to registration
  if (roles.length === 0) {
    router.navigate(['/registration']);
    return false;
  }

  return true;
};
