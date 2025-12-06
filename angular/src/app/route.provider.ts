import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
      path: '/',
      name: '::Menu:Home',
      iconClass: 'fas fa-home',
      order: 1,
      layout: eLayoutType.application,
    },
    {
      path: '/trips/available',
      name: 'Available Trips',
      iconClass: 'fas fa-route',
      order: 2,
      layout: eLayoutType.application,
      requiredPolicy: 'Naqliyat.Drivers', // For drivers
    },
    {
      path: '/trips/my-trips',
      name: 'My Trips',
      iconClass: 'fas fa-truck-loading',
      order: 3,
      layout: eLayoutType.application,
      requiredPolicy: 'Naqliyat.Drivers', // For drivers
    },
    {
      path: '/trips/create',
      name: 'Create Trip',
      iconClass: 'fas fa-plus-circle',
      order: 4,
      layout: eLayoutType.application,
      requiredPolicy: 'Naqliyat.Trips.Create', // For service requesters
    },
    {
      path: '/trips/list',
      name: 'My Requests',
      iconClass: 'fas fa-list-alt',
      order: 5,
      layout: eLayoutType.application,
      requiredPolicy: 'Naqliyat.Trips', // For service requesters
    },
    {
      path: '/trucks',
      name: 'My Trucks',
      iconClass: 'fas fa-truck',
      order: 6,
      layout: eLayoutType.application,
      requiredPolicy: 'Naqliyat.Trucks', // For truck owners
    },
    {
      path: '/drivers',
      name: 'Drivers',
      iconClass: 'fas fa-id-card',
      order: 7,
      layout: eLayoutType.application,
      requiredPolicy: 'Naqliyat.Drivers.Manage', // For truck owners/admins
    },
  ]);
}
