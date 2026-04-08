import { Link, useLocation } from 'react-router-dom';
function Navbar() {
  const loc = useLocation();
  const active = { borderBottom: '3px solid white', paddingBottom: '4px' };
  return (
    <nav style={{ background: '#083F75', padding: '0 32px', display: 'flex', alignItems: 'center', height: '60px', boxShadow: '0 2px 8px rgba(0,0,0,0.2)' }}>
      <span style={{ color: 'white', fontWeight: '700', fontSize: '20px', marginRight: '40px', letterSpacing: '1px' }}>SIGEBI</span>
      <div style={{ display: 'flex', gap: '8px' }}>
        {[
          { to: '/recursos', label: 'Recursos' },
          { to: '/prestamos', label: 'Mis Prestamos' },
          { to: '/penalizaciones', label: 'Mis Penalizaciones' }
        ].map(item => (
          <Link key={item.to} to={item.to} style={{ color: 'white', textDecoration: 'none', padding: '8px 16px', borderRadius: '6px', fontSize: '14px', background: loc.pathname === item.to ? 'rgba(255,255,255,0.15)' : 'transparent', ...(loc.pathname === item.to ? active : {}) }}>
            {item.label}
          </Link>
        ))}
      </div>
      <div style={{ marginLeft: 'auto' }}>
        <button onClick={() => { localStorage.removeItem('token'); window.location.href = '/'; }} style={{ background: 'rgba(255,255,255,0.15)', color: 'white', border: '1px solid rgba(255,255,255,0.3)', borderRadius: '6px', padding: '6px 14px', cursor: 'pointer', fontSize: '13px' }}>
          Cerrar sesion
        </button>
      </div>
    </nav>
  );
}
export default Navbar;
