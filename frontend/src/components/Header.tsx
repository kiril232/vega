import { Link, NavLink } from 'react-router-dom';
import { useAuth } from '../state/AuthContext';
import { useCart } from '../state/CartContext';
import './Header.css';

export default function Header() {
  const { user, logout } = useAuth();
  const { cart } = useCart();
  const itemCount = cart?.items.reduce((sum, i) => sum + i.quantity, 0) ?? 0;

  return (
    <header className="vega-header">
      <div className="container vega-header-row">
        <Link to="/" className="vega-logo">
          <span className="vega-logo-mark">V</span>
          <span>Vega v2</span>
          <small>Agricultural Pharmacy</small>
        </Link>

        <nav className="vega-nav">
          <NavLink to="/" end>Shop</NavLink>
          <NavLink to="/orders">Orders</NavLink>
        </nav>

        <div className="vega-actions">
          {user ? (
            <>
              <span className="muted vega-greeting">Hi, {user.fullName.split(' ')[0]}</span>
              <button className="btn-ghost" onClick={logout}>Sign out</button>
            </>
          ) : (
            <>
              <Link to="/login" className="btn-ghost">Sign in</Link>
              <Link to="/register" className="btn btn-outline">Register</Link>
            </>
          )}
          <Link to="/cart" className="vega-cart-pill" aria-label="Cart">
            <span>Cart</span>
            <span className="vega-cart-badge">{itemCount}</span>
          </Link>
        </div>
      </div>
    </header>
  );
}
