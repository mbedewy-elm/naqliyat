import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';

export const TRIP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'create',
  },
  {
    path: 'create',
    loadComponent: () => import('./create-trip/create-trip.component').then(c => c.CreateTripComponent),
    canActivate: [authGuard],
  },
  {
    path: 'list',
    loadComponent: () => import('./trip-list/trip-list.component').then(c => c.TripListComponent),
    canActivate: [authGuard],
  },
  {
    path: 'available',
    loadComponent: () => import('./driver-trips/driver-trips.component').then(c => c.DriverTripsComponent),
    canActivate: [authGuard],
  },
  {
    path: 'my-trips',
    loadComponent: () => import('./my-trips/my-trips.component').then(c => c.MyTripsComponent),
    canActivate: [authGuard],
  },
  {
    path: 'bid/:id',
    loadComponent: () => import('./place-bid/place-bid.component').then(c => c.PlaceBidComponent),
    canActivate: [authGuard],
  },
  {
    path: ':id',
    loadComponent: () => import('./trip-detail/trip-detail.component').then(c => c.TripDetailComponent),
    canActivate: [authGuard],
  },
];
