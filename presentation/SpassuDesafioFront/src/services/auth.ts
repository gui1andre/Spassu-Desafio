import { api } from './api';
import type { AuthResponse } from '../types/api';

export const authService = {
  login: (email: string, password: string) =>
    api.post<AuthResponse>('/Auth/login', { email, password }),

  register: (email: string, password: string, confirmPassword: string) =>
    api.post<AuthResponse>('/Auth/register', { email, password, confirmPassword }),

  refreshToken: (email: string) =>
    api.post<AuthResponse>(`/Auth/refresh-token?email=${encodeURIComponent(email)}`, {}),
};
