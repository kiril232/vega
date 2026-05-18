export type AuthResponse = {
  token: string;
  user: UserResponse;
};

export type UserResponse = {
  id: string;
  email: string;
  fullName: string;
};

export type Product = {
  id: string;
  name: string;
  description: string;
  category: string;
  price: number;
  stock: number;
  imageUrl: string;
};

export type CartItem = {
  id: string;
  productId: string;
  productName: string;
  imageUrl: string;
  price: number;
  quantity: number;
  lineTotal: number;
};

export type Cart = {
  id: string;
  userId: string;
  items: CartItem[];
  total: number;
};

export type OrderItem = {
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
};

export type Order = {
  id: string;
  userId: string;
  total: number;
  currency: string;
  status: 'Created' | 'Paid' | 'Failed';
  failureReason: string | null;
  createdAt: string;
  items: OrderItem[];
};
