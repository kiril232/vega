import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useCart } from '../state/CartContext';
import './cart.css';

export default function CartPage() {
  const { cart, update, remove, loading } = useCart();
  const [busy, setBusy] = useState<string | null>(null);
  const navigate = useNavigate();

  async function setQty(productId: string, qty: number) {
    setBusy(productId);
    try {
      await update(productId, qty);
    } finally {
      setBusy(null);
    }
  }

  if (loading && !cart) {
    return <div className="container muted" style={{ padding: 40 }}>Loading cart…</div>;
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div className="container cart-empty">
        <h1>Your cart is empty</h1>
        <p className="muted">Browse the shop and add some products.</p>
        <Link to="/" className="btn">Continue shopping</Link>
      </div>
    );
  }

  return (
    <div className="container cart">
      <h1>Shopping cart</h1>
      <div className="cart-grid">
        <div className="cart-items">
          {cart.items.map((item) => (
            <div key={item.id} className="cart-item card">
              <img
                src={item.imageUrl || '/placeholder.svg'}
                alt={item.productName}
                onError={(e) => (e.currentTarget.src = '/placeholder.svg')}
              />
              <div className="cart-item-body">
                <div className="cart-item-title">{item.productName}</div>
                <div className="muted">€ {item.price.toFixed(2)} per unit</div>

                <div className="cart-item-controls">
                  <div className="qty-stepper">
                    <button
                      onClick={() => setQty(item.productId, Math.max(0, item.quantity - 1))}
                      disabled={busy === item.productId}
                    >
                      −
                    </button>
                    <input value={item.quantity} readOnly />
                    <button
                      onClick={() => setQty(item.productId, item.quantity + 1)}
                      disabled={busy === item.productId}
                    >
                      +
                    </button>
                  </div>
                  <button
                    className="btn-ghost"
                    onClick={() => remove(item.productId)}
                    disabled={busy === item.productId}
                  >
                    Remove
                  </button>
                </div>
              </div>
              <div className="cart-item-line">€ {item.lineTotal.toFixed(2)}</div>
            </div>
          ))}
        </div>

        <aside className="cart-summary card">
          <h3>Order summary</h3>
          <div className="row">
            <span className="muted">Items</span>
            <span>{cart.items.reduce((s, i) => s + i.quantity, 0)}</span>
          </div>
          <div className="row">
            <span className="muted">Subtotal</span>
            <span>€ {cart.total.toFixed(2)}</span>
          </div>
          <div className="row">
            <span className="muted">Shipping</span>
            <span>Free</span>
          </div>
          <div className="row total">
            <span>Total</span>
            <span>€ {cart.total.toFixed(2)}</span>
          </div>
          <button className="btn" onClick={() => navigate('/checkout')}>
            Proceed to checkout
          </button>
        </aside>
      </div>
    </div>
  );
}
