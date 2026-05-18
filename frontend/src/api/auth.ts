import { userApi } from './client';
import { AuthResponse, UserResponse } from './types';

export function register(email: string, password: string, fullName: string) {
  return userApi.post<AuthResponse>('/auth/register', { email, password, fullName }).then((r) => r.data);
}

export function login(email: string, password: string) {
  return userApi.post<AuthResponse>('/auth/login', { email, password }).then((r) => r.data);
}

export function me() {
  return userApi.get<UserResponse>('/users/me').then((r) => r.data);
}
