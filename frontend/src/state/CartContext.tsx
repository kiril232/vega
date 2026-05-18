import { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { Cart } from '../api/types';
import * as cartApi from '../api/cart';
import { useAuth } from './AuthContext';

type CartState = {
  cart: Cart | null;
  loading: boolean;
  refresh: () => Promise<void>;
  add: (productId: string, quantity: number) => Promise<void>;
  update: (productId: string, quantity: number) => Promise<void>;
  remove: (productId: string) => Promise<void>;
  clear: () => Promise<void>;
};

const CartCtx = createContext<CartState | undefined>(undefined);

export function CartProvider({ children }: { children: React.ReactNode }) {
  const { token } = useAuth();
  const [cart, setCart] = useState<Cart | null>(null);
  const [loading, setLoading] = useState(false);

  const refresh = useCallback(async () => {
    if (!token) {
      setCart(null);
      return;
    }
    setLoading(true);
    try {
      setCart(await cartApi.getCart());
    } finally {
      setLoading(false);
    }
  }, [token]);

  // pull a fresh cart whenever the user signs in or out
  useEffect(() => {
    refresh().catch(() => undefined);
  }, [refresh]);

  const add = useCallback(async (productId: string, quantity: number) => {
    const c = await cartApi.addItem(productId, quantity);
    setCart(c);
  }, []);

  const update = useCallback(async (productId: string, quantity: number) => {
    const c = await cartApi.updateItem(productId, quantity);
    setCart(c);
  }, []);

  const remove = useCallback(async (productId: string) => {
    const c = await cartApi.removeItem(productId);
    setCart(c);
  }, []);

  const clear = useCallback(async () => {
    const c = await cartApi.clearCart();
    setCart(c);
  }, []);

  const value = useMemo<CartState>(
    () => ({ cart, loading, refresh, add, update, remove, clear }),
    [cart, loading, refresh, add, update, remove, clear]
  );

  return <CartCtx.Provider value={value}>{children}</CartCtx.Provider>;
}

export function useCart() {
  const ctx = useContext(CartCtx);
  if (!ctx) throw new Error('useCart must be used inside CartProvider');
  return ctx;
}
