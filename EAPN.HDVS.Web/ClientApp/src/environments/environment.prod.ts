import { env } from 'process';

export const environment = {
  production: true,
  apiEndpoint: env.API_ENDPOINT,
  tokenExcludeEndpoints: ['api/auth/refresh', 'api/auth'],
  tokenLocalStorageKey: 'HDVSApiToken',
};
