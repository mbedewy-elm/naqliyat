import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';

export const TRUCK_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'list',
  },
  {
    path: 'list',
    loadComponent: () => import('./truck-list/truck-list.component').then(c => c.TruckListComponent),
    canActivate: [authGuard],
  },
  {
    path: 'add',
    loadComponent: () => import('./add-truck/add-truck.component').then(c => c.AddTruckComponent),
    canActivate: [authGuard],
  },
  {
    path: ':id',
    loadComponent: () => import('./truck-detail/truck-detail.component').then(c => c.TruckDetailComponent),
    canActivate: [authGuard],
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./edit-truck/edit-truck.component').then(c => c.EditTruckComponent),
    canActivate: [authGuard],
  },
];
