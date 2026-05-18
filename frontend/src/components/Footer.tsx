import './Footer.css';

export default function Footer() {
  return (
    <footer className="vega-footer">
      <div className="container vega-footer-row">
        <div>
          <strong>Vega</strong>
          <p className="muted">Agricultural pharmacy supplying veterinary, crop, and livestock care.</p>
        </div>
        <div>
          <h4>Shop</h4>
          <ul>
            <li>Livestock</li>
            <li>Crop Protection</li>
            <li>Feed Additives</li>
            <li>Equipment</li>
          </ul>
        </div>
        <div>
          <h4>Help</h4>
          <ul>
            <li>Shipping</li>
            <li>Returns</li>
            <li>Contact</li>
          </ul>
        </div>
      </div>
      <div className="vega-footer-meta">
        <div className="container muted">© {new Date().getFullYear()} Vega. Demo project.</div>
      </div>
    </footer>
  );
}
