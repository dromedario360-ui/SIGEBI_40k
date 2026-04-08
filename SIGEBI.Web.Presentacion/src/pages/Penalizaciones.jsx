import { useEffect, useState } from 'react';
import { getPenalizacionesPorUsuario } from '../services/penalizacionService';
function Penalizaciones() {
  const [penalizaciones, setPenalizaciones] = useState([]);
  const [error, setError] = useState('');
  useEffect(() => {
    getPenalizacionesPorUsuario(1).then(res => setPenalizaciones(res.data)).catch(() => setError('Error al cargar penalizaciones'));
  }, []);
  return (
    <div className='card'>
      <h2 className='page-title'>Mis Penalizaciones</h2>
      {error && <div className='error-msg'>{error}</div>}
      <div style={{ borderRadius: '8px', overflow: 'hidden', border: '1px solid #e0e0e0' }}>
        <table>
          <thead><tr><th>ID</th><th>Motivo</th><th>Monto</th><th>Fecha Inicio</th><th>Estado</th></tr></thead>
          <tbody>
            {penalizaciones.length === 0 ? (
              <tr><td colSpan='5' className='empty-msg'>No tienes penalizaciones registradas</td></tr>
            ) : penalizaciones.map(p => (
              <tr key={p.id}>
                <td style={{ fontFamily: 'monospace' }}>#{p.id}</td>
                <td>{p.motivo}</td>
                <td style={{ color: '#c0392b', fontWeight: '500' }}>RD{p.monto}</td>
                <td>{new Date(p.fechaInicio).toLocaleDateString('es-DO')}</td>
                <td><span className={p.activa ? 'badge-no' : 'badge-si'}>{p.activa ? 'Activa' : 'Finalizada'}</span></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
export default Penalizaciones;
