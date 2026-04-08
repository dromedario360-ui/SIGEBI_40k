import { useState } from 'react';
import api from '../services/api';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const res = await api.post('/Auth/login', { email, password });
      localStorage.setItem('idUsuario', res.data.id);
      localStorage.setItem('nombre', res.data.nombre);
      localStorage.setItem('rol', res.data.rol);
      window.location.href = '/recursos';
    } catch {
      setError('Correo o contrasena incorrectos');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ minHeight: '100vh', background: 'linear-gradient(135deg, #083F75 0%, #1061B0 100%)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <div style={{ background: 'white', borderRadius: '16px', padding: '48px 40px', width: '100%', maxWidth: '420px', boxShadow: '0 20px 60px rgba(0,0,0,0.3)' }}>
        <div style={{ textAlign: 'center', marginBottom: '32px' }}>
          <div style={{ background: '#083F75', color: 'white', width: '60px', height: '60px', borderRadius: '50%', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 16px', fontSize: '24px', fontWeight: 'bold' }}>S</div>
          <h1 style={{ color: '#083F75', fontSize: '26px', fontWeight: '700' }}>SIGEBI</h1>
          <p style={{ color: '#888', fontSize: '14px', marginTop: '4px' }}>Sistema de Gestion de Biblioteca</p>
        </div>
        {error && <div className='error-msg'>{error}</div>}
        <form onSubmit={handleLogin}>
          <div style={{ marginBottom: '16px' }}>
            <label style={{ display: 'block', fontSize: '13px', fontWeight: '500', color: '#444', marginBottom: '6px' }}>Correo electronico</label>
            <input type='email' value={email} onChange={e => setEmail(e.target.value)} required placeholder='ejemplo@correo.com' style={{ width: '100%', padding: '10px 14px', border: '1px solid #ddd', borderRadius: '8px', fontSize: '14px', outline: 'none' }} />
          </div>
          <div style={{ marginBottom: '24px' }}>
            <label style={{ display: 'block', fontSize: '13px', fontWeight: '500', color: '#444', marginBottom: '6px' }}>Contrasena</label>
            <input type='password' value={password} onChange={e => setPassword(e.target.value)} required placeholder='••••••••' style={{ width: '100%', padding: '10px 14px', border: '1px solid #ddd', borderRadius: '8px', fontSize: '14px', outline: 'none' }} />
          </div>
          <button type='submit' disabled={loading} style={{ width: '100%', padding: '12px', background: '#083F75', color: 'white', border: 'none', borderRadius: '8px', fontSize: '15px', fontWeight: '600', cursor: 'pointer' }}>
            {loading ? 'Ingresando...' : 'Ingresar'}
          </button>
        </form>
      </div>
    </div>
  );
}

export default Login;