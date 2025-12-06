import { Environment } from '@abp/ng.core';

// For Android development: use ngrok URL
// For production: use your actual server URL
const backendUrl = 'https://1b9a24c3ce39.ngrok-free.app';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: backendUrl + '/',
  redirectUri: baseUrl,
  clientId: 'Naqliyat_App',
  responseType: 'code',
  scope: 'offline_access Naqliyat',
  requireHttps: false, // Set to false for development with IP addresses
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Naqliyat',
  },
  oAuthConfig,
  apis: {
    default: {
      url: backendUrl,
      rootNamespace: 'Naqliyat',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;
