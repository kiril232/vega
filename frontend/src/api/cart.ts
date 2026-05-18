import { cartApi } from './client';
import { Cart } from './types';

export function getCart() {
  return cartApi.get<Cart>('/cart').then((r) => r.data);
}

export function addItem(productId: string, quantity: number) {
  return cartApi.post<Cart>('/cart/items', { productId, quantity }).then((r) => r.data);
}

export function updateItem(productId: string, quantity: number) {
  return cartApi.put<Cart>(`/cart/items/${productId}`, { quantity }).then((r) => r.data);
}

export function removeItem(productId: string) {
  return cartApi.delete<Cart>(`/cart/items/${productId}`).then((r) => r.data);
}

export function clearCart() {
  return cartApi.delete<Cart>('/cart/clear').then((r) => r.data);
}
