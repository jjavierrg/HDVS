export const environment = {
  production: true,
  apiEndpoint: location.origin,
  tokenExcludeEndpoints: ['api/auth/refresh', 'api/auth'],
  tokenLocalStorageKey: 'HDVSApiToken',
};
