import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import * as productsApi from '../api/products';
import { Product } from '../api/types';
import { useAuth } from '../state/AuthContext';
import { useCart } from '../state/CartContext';
import './product.css';

export default function ProductPage() {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<Product | null>(null);
  const [qty, setQty] = useState(1);
  const [error, setError] = useState<string | null>(null);
  const [adding, setAdding] = useState(false);

  const { token } = useAuth();
  const { add } = useCart();
  const navigate = useNavigate();

  useEffect(() => {
    if (!id) return;
    productsApi.getProduct(id).then(setProduct).catch(() => setError('product not found'));
  }, [id]);

  if (error) return <div className="container"><div className="notice error">{error}</div></div>;
  if (!product) return <div className="container muted" style={{ padding: 40 }}>Loading…</div>;

  async function onAdd() {
    if (!token) {
      navigate('/login');
      return;
    }
    if (!product) return;
    setAdding(true);
    try {
      await add(product.id, qty);
      navigate('/cart');
    } catch (e: any) {
      setError(e?.response?.data?.error ?? 'could not add to cart');
    } finally {
      setAdding(false);
    }
  }

  return (
    <div className="container product-detail">
      <div className="product-detail-grid">
        <div className="product-detail-image">
          <img src={product.imageUrl || '/placeholder.svg'} alt={product.name} onError={(e) => (e.currentTarget.src = '/placeholder.svg')} />
        </div>
        <div className="product-detail-body">
          <span className="badge">{product.category}</span>
          <h1>{product.name}</h1>
          <p className="muted">{product.description}</p>

          <div className="price-row">
            <span className="price-large">€ {product.price.toFixed(2)}</span>
            <span className="muted">
              {product.stock > 0 ? `${product.stock} in stock` : 'out of stock'}
            </span>
          </div>

          <div className="qty-row">
            <label htmlFor="qty">Quantity</label>
            <div className="qty-stepper">
              <button onClick={() => setQty((q) => Math.max(1, q - 1))} aria-label="decrease">−</button>
              <input
                id="qty"
                type="number"
                value={qty}
                min={1}
                max={product.stock}
                onChange={(e) => setQty(Math.max(1, Number(e.target.value)))}
              />
              <button onClick={() => setQty((q) => Math.min(product.stock, q + 1))} aria-label="increase">+</button>
            </div>
          </div>

          <button className="btn" disabled={adding || product.stock <= 0} onClick={onAdd}>
            {adding ? 'Adding…' : 'Add to cart'}
          </button>
        </div>
      </div>
    </div>
  );
}
