export const PAGINATION = {
  DEFAULT_PAGE_SIZE: 20,
  PAGE_SIZE_OPTIONS: [10, 20, 50, 100]
} as const;

export const API_TIMEOUTS = {
  DEFAULT: 30000,
  LONG_RUNNING: 60000
} as const;
