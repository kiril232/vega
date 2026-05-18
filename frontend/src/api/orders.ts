import { orderApi } from './client';
import { Order } from './types';

export function placeOrder() {
  return orderApi.post<Order>('/orders').then((r) => r.data);
}

export function getOrder(id: string) {
  return orderApi.get<Order>(`/orders/${id}`).then((r) => r.data);
}

export function listMyOrders(userId: string) {
  return orderApi.get<Order[]>(`/orders/user/${userId}`).then((r) => r.data);
}
