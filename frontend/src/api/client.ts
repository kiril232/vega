import axios, { AxiosInstance } from 'axios';

// each microservice has its own base url; we use one axios instance per
// service so interceptors stay simple
function make(baseURL: string): AxiosInstance {
  const inst = axios.create({ baseURL });
  inst.interceptors.request.use((config) => {
    const token = localStorage.getItem('vega.token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });
  inst.interceptors.response.use(
    (r) => r,
    (err) => {
      if (err?.response?.status === 401) {
        // boot stale session; let the app re-route on next render
        localStorage.removeItem('vega.token');
        localStorage.removeItem('vega.user');
      }
      return Promise.reject(err);
    }
  );
  return inst;
}

export const userApi = make(import.meta.env.VITE_USER_API ?? 'http://localhost:5001');
export const productApi = make(import.meta.env.VITE_PRODUCT_API ?? 'http://localhost:5002');
export const cartApi = make(import.meta.env.VITE_CART_API ?? 'http://localhost:5003');
export const orderApi = make(import.meta.env.VITE_ORDER_API ?? 'http://localhost:5004');
