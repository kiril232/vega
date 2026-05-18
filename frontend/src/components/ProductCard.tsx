import { Link } from 'react-router-dom';
import { Product } from '../api/types';
import './ProductCard.css';

type Props = {
  product: Product;
  onAdd?: (p: Product) => void;
};

export default function ProductCard({ product, onAdd }: Props) {
  return (
    <article className="product-card">
      <Link to={`/products/${product.id}`} className="product-image">
        <img src={product.imageUrl || '/placeholder.svg'} alt={product.name} onError={onImgError} />
      </Link>
      <div className="product-body">
        <span className="badge">{product.category}</span>
        <Link to={`/products/${product.id}`} className="product-name">{product.name}</Link>
        <p className="muted product-desc">{product.description}</p>
        <div className="product-bottom">
          <strong className="product-price">€ {product.price.toFixed(2)}</strong>
          <button
            className="btn"
            disabled={product.stock <= 0}
            onClick={() => onAdd?.(product)}
          >
            {product.stock > 0 ? 'Add to cart' : 'Out of stock'}
          </button>
        </div>
      </div>
    </article>
  );
}

function onImgError(e: React.SyntheticEvent<HTMLImageElement>) {
  e.currentTarget.src = '/placeholder.svg';
}
