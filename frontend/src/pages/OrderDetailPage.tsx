import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import * as ordersApi from '../api/orders';
import { Order } from '../api/types';
import './orders.css';

export default function OrderDetailPage() {
  const { id } = useParams<{ id: string }>();
  const [order, setOrder] = useState<Order | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;
    ordersApi.getOrder(id).then(setOrder).catch(() => setError('order not found'));
  }, [id]);

  if (error) return <div className="container"><div className="notice error">{error}</div></div>;
  if (!order) return <div className="container muted" style={{ padding: 40 }}>Loading…</div>;

  return (
    <div className="container order-detail">
      <Link to="/orders" className="btn-ghost back-link">← All orders</Link>

      <div className="order-header card">
        <div>
          <div className="muted small">Order</div>
          <h1>#{order.id.slice(0, 8).toUpperCase()}</h1>
          <div className="muted small">Placed {new Date(order.createdAt).toLocaleString()}</div>
        </div>
        <div className="order-header-status">
          <span className={`status status-${order.status.toLowerCase()}`}>{order.status}</span>
          <div className="muted small" style={{ marginTop: 8 }}>
            {order.failureReason ?? ''}
          </div>
        </div>
      </div>

      <div className="card order-items">
        {order.items.map((i) => (
          <div key={i.productId} className="order-item-row">
            <div>
              <div className="cart-item-title">{i.productName}</div>
              <div className="muted small">€ {i.unitPrice.toFixed(2)} × {i.quantity}</div>
            </div>
            <strong>€ {i.lineTotal.toFixed(2)}</strong>
          </div>
        ))}
        <div className="order-item-row total">
          <span>Total</span>
          <strong>€ {order.total.toFixed(2)}</strong>
        </div>
      </div>
    </div>
  );
}
