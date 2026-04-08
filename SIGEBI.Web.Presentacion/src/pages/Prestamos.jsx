import { useEffect, useState } from 'react';
import { getPrestamosPorUsuario } from '../services/prestamoService';

function Prestamos() {
  const [prestamos, setPrestamos] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    getPrestamosPorUsuario(parseInt(localStorage.getItem('idUsuario')))
      .then(res => setPrestamos(res.data))
      .catch(() => setError('Error al cargar prestamos'));
  }, []);

  const getBadge = (estado) => {
    if (estado === 'ACTIVO') return 'badge-activo';
    if (estado === 'DEVUELTO') return 'badge-devuelto';
    return 'badge-vencido';
  };

  return (
    <div className='card'>
      <h2 className='page-title'>Mis Prestamos</h2>
      {error && <div className='error-msg'>{error}</div>}
      <div style={{ borderRadius: '8px', overflow: 'hidden', border: '1px solid #e0e0e0' }}>
        <table>
          <thead>
            <tr>
              <th>ID</th><th>Fecha Prestamo</th><th>Fecha Limite</th><th>Estado</th><th>Multa</th>
            </tr>
          </thead>
          <tbody>
            {prestamos.length === 0 ? (
              <tr><td colSpan='5' className='empty-msg'>No tienes prestamos registrados</td></tr>
            ) : prestamos.map(p => (
              <tr key={p.id}>
                <td style={{ fontFamily: 'monospace' }}>#{p.id}</td>
                <td>{new Date(p.fechaPrestamo).toLocaleDateString('es-DO')}</td>
                <td>{new Date(p.fechaLimite).toLocaleDateString('es-DO')}</td>
                <td><span className={getBadge(p.estado)}>{p.estado}</span></td>
                <td style={{ color: p.totalMulta > 0 ? '#c0392b' : '#27ae60', fontWeight: '500' }}>
                  {p.totalMulta > 0 ? 'RD$' + p.totalMulta : 'Sin multa'}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Prestamos;