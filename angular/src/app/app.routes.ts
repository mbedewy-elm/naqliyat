import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'registration',
    loadChildren: () => import('./registration/registration.routes').then(c => c.REGISTRATION_ROUTES),
  },
  {
    path: 'trip',
    loadChildren: () => import('./trip/trip.routes').then(c => c.TRIP_ROUTES),
  },
  {
    path: 'truck',
    loadChildren: () => import('./truck/truck.routes').then(c => c.TRUCK_ROUTES),
  },
  {
    path: 'driver',
    loadChildren: () => import('./driver/driver.routes').then(c => c.DRIVER_ROUTES),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(c => c.createRoutes()),
  },
];
