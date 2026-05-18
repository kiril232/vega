import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import * as ordersApi from '../api/orders';
import { Order } from '../api/types';
import { useAuth } from '../state/AuthContext';
import './orders.css';

export default function OrdersPage() {
  const { user } = useAuth();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!user) return;
    ordersApi
      .listMyOrders(user.id)
      .then(setOrders)
      .finally(() => setLoading(false));
  }, [user]);

  if (loading) return <div className="container muted" style={{ padding: 40 }}>Loading orders…</div>;

  if (orders.length === 0) {
    return (
      <div className="container orders-empty">
        <h1>No orders yet</h1>
        <p className="muted">When you place an order it will show up here.</p>
        <Link to="/" className="btn">Start shopping</Link>
      </div>
    );
  }

  return (
    <div className="container orders">
      <h1>Your orders</h1>
      <div className="orders-list">
        {orders.map((o) => (
          <Link key={o.id} to={`/orders/${o.id}`} className="card order-row">
            <div>
              <div className="order-id">#{o.id.slice(0, 8).toUpperCase()}</div>
              <div className="muted small">
                {new Date(o.createdAt).toLocaleDateString()} · {o.items.length} item{o.items.length === 1 ? '' : 's'}
              </div>
            </div>
            <span className={`status status-${o.status.toLowerCase()}`}>{o.status}</span>
            <strong>€ {o.total.toFixed(2)}</strong>
          </Link>
        ))}
      </div>
    </div>
  );
}
