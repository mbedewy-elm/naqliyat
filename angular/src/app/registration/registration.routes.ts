import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';

export const REGISTRATION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./role-selection/role-selection.component').then(c => c.RoleSelectionComponent),
    canActivate: [authGuard],
  },
  {
    path: 'owner',
    loadComponent: () => import('./register-owner/register-owner.component').then(c => c.RegisterOwnerComponent),
    canActivate: [authGuard],
  },
  {
    path: 'driver',
    loadComponent: () => import('./register-driver/register-driver.component').then(c => c.RegisterDriverComponent),
    canActivate: [authGuard],
  },
];
