import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';

export const DRIVER_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'list',
  },
  {
    path: 'list',
    loadComponent: () => import('./driver-list/driver-list.component').then(c => c.DriverListComponent),
    canActivate: [authGuard],
  },
  {
    path: 'add',
    loadComponent: () => import('./add-driver/add-driver.component').then(c => c.AddDriverComponent),
    canActivate: [authGuard],
  },
  {
    path: ':id',
    loadComponent: () => import('./driver-detail/driver-detail.component').then(c => c.DriverDetailComponent),
    canActivate: [authGuard],
  },
];
