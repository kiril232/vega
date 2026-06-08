import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import * as productsApi from '../api/products';
import { Product } from '../api/types';
import ProductCard from '../components/ProductCard';
import { useAuth } from '../state/AuthContext';
import { useCart } from '../state/CartContext';
import './home.css';

export default function HomePage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [category, setCategory] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [toast, setToast] = useState<string | null>(null);

  const { token } = useAuth();
  const { add } = useCart();
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    productsApi
      .listProducts(category ?? undefined)
      .then(setProducts)
      .catch(() => setError('could not load products'))
      .finally(() => setLoading(false));
  }, [category]);

  const categories = useMemo(() => {
    const set = new Set(products.map((p) => p.category));
    return Array.from(set).sort();
  }, [products]);

  async function handleAdd(p: Product) {
    if (!token) {
      navigate('/login', { state: { from: '/' } });
      return;
    }
    try {
      await add(p.id, 1);
      setToast(`${p.name} added to cart`);
      setTimeout(() => setToast(null), 1800);
    } catch {
      setError('could not add to cart');
    }
  }

  return (
    <div className="container home">
      <section className="hero">
        <div>
          <h1>Veterinary &amp; agricultural pharmacy</h1>
          <p className="muted">
            Trusted supplies for livestock, crops, and on-farm care. Delivered to your door.
          </p>
          <div className="hero-stats">
            <div className="hero-stat">
              <strong>200+</strong>
              <span>Products</span>
            </div>
            <div className="hero-stat">
              <strong>8</strong>
              <span>Categories</span>
            </div>
            <div className="hero-stat">
              <strong>Next day</strong>
              <span>Delivery</span>
            </div>
          </div>
        </div>
        <div className="hero-art" aria-hidden>
          <div className="hero-circle" />
        </div>
      </section>

      <p className="filters-label">Browse by category</p>
      <section className="filters">
        <button
          className={`filter-pill ${category === null ? 'active' : ''}`}
          onClick={() => setCategory(null)}
        >
          All
        </button>
        {categories.map((c) => (
          <button
            key={c}
            className={`filter-pill ${category === c ? 'active' : ''}`}
            onClick={() => setCategory(c)}
          >
            {c}
          </button>
        ))}
      </section>

      {error && <div className="notice error">{error}</div>}
      {toast && <div className="notice info toast">{toast}</div>}

      {loading ? (
        <div className="muted">Loading products…</div>
      ) : (
        <>
          <p className="grid-heading">
            {category ? category : 'All products'} &nbsp;
            <span className="muted" style={{ fontWeight: 400, fontSize: '14px' }}>
              ({products.length})
            </span>
          </p>
          <div className="grid">
            {products.map((p) => (
              <ProductCard key={p.id} product={p} onAdd={handleAdd} />
            ))}
          </div>
        </>
      )}
    </div>
  );
}
