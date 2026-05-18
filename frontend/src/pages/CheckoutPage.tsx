import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import * as ordersApi from '../api/orders';
import { useCart } from '../state/CartContext';
import { useAuth } from '../state/AuthContext';
import './checkout.css';

export default function CheckoutPage() {
  const { cart, refresh } = useCart();
  const { user } = useAuth();
  const navigate = useNavigate();
  const [placing, setPlacing] = useState(false);
  const [error, setError] = useState<string | null>(null);

  if (!cart || cart.items.length === 0) {
    return (
      <div className="container checkout-empty">
        <h1>Nothing to check out</h1>
        <p className="muted">Add items to your cart first.</p>
        <Link to="/" className="btn">Back to shop</Link>
      </div>
    );
  }

  async function placeOrder() {
    setPlacing(true);
    setError(null);
    try {
      const order = await ordersApi.placeOrder();
      // cart was cleared server-side on a paid order; either way pull fresh state
      await refresh().catch(() => undefined);
      navigate(`/orders/${order.id}`);
    } catch (e: any) {
      setError(e?.response?.data?.error ?? 'order failed');
    } finally {
      setPlacing(false);
    }
  }

  return (
    <div className="container checkout">
      <h1>Checkout</h1>
      <div className="checkout-grid">
        <section className="card checkout-summary">
          <h3>Review your order</h3>
          <ul>
            {cart.items.map((i) => (
              <li key={i.id}>
                <span>{i.productName} <span className="muted">× {i.quantity}</span></span>
                <strong>€ {i.lineTotal.toFixed(2)}</strong>
              </li>
            ))}
          </ul>
          <div className="row total">
            <span>Total</span>
            <span>€ {cart.total.toFixed(2)}</span>
          </div>
        </section>

        <aside className="card checkout-side">
          <h3>Delivery</h3>
          <p className="muted">Sending to <strong>{user?.fullName}</strong> · {user?.email}</p>

          <h3 style={{ marginTop: 20 }}>Payment</h3>
          <p className="muted">Sandbox checkout — no card required for this demo.</p>

          {error && <div className="notice error">{error}</div>}

          <button className="btn" disabled={placing} onClick={placeOrder}>
            {placing ? 'Placing order…' : `Pay € ${cart.total.toFixed(2)}`}
          </button>
          <Link to="/cart" className="btn-ghost back">Back to cart</Link>
        </aside>
      </div>
    </div>
  );
}
