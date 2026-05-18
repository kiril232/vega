import { productApi } from './client';
import { Product } from './types';

export function listProducts(category?: string) {
  return productApi
    .get<Product[]>('/products', { params: category ? { category } : undefined })
    .then((r) => r.data);
}

export function getProduct(id: string) {
  return productApi.get<Product>(`/products/${id}`).then((r) => r.data);
}
