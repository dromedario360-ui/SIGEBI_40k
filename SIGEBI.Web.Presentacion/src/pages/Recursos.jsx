import { useEffect, useState } from 'react';
import { getRecursos } from '../services/recursoService';
import { crearPrestamo } from '../services/prestamoService';

function Recursos() {
  const [recursos, setRecursos] = useState([]);
  const [error, setError] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [loading, setLoading] = useState(null);

  useEffect(() => {
    getRecursos().then(res => setRecursos(res.data)).catch(() => setError('Error al cargar recursos'));
  }, []);

  const filtrados = recursos.filter(r =>
    r.titulo.toLowerCase().includes(busqueda.toLowerCase()) ||
    r.autor.toLowerCase().includes(busqueda.toLowerCase()) ||
    r.codigo.toLowerCase().includes(busqueda.toLowerCase())
  );

  const handleSolicitar = async (recurso) => {
    setLoading(recurso.id);
    setMensaje('');
    setError('');
    try {
      const fechaLimite = new Date();
      fechaLimite.setDate(fechaLimite.getDate() + 7);
      await crearPrestamo({
        idUsuario: parseInt(localStorage.getItem('idUsuario')),
        fechaLimite: fechaLimite.toISOString(),
        detalles: [{ idRecurso: recurso.id, cantidad: 1 }]
      });
      setMensaje(`Prestamo de "${recurso.titulo}" solicitado exitosamente.`);
      getRecursos().then(res => setRecursos(res.data));
    } catch {
      setError('Error al solicitar el prestamo.');
    } finally {
      setLoading(null);
    }
  };

  return (
    <div className='card'>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2 className='page-title' style={{ margin: 0 }}>Catalogo de Recursos</h2>
        <input placeholder='Buscar por titulo, autor o codigo...' value={busqueda} onChange={e => setBusqueda(e.target.value)} style={{ padding: '8px 14px', border: '1px solid #ddd', borderRadius: '8px', fontSize: '14px', width: '280px' }} />
      </div>
      {error && <div className='error-msg'>{error}</div>}
      {mensaje && <div style={{ background: '#d4edda', color: '#155724', padding: '10px 16px', borderRadius: '8px', marginBottom: '16px' }}>{mensaje}</div>}
      <div style={{ borderRadius: '8px', overflow: 'hidden', border: '1px solid #e0e0e0' }}>
        <table>
          <thead>
            <tr>
              <th>Codigo</th><th>Titulo</th><th>Autor</th><th>Tipo</th><th>Stock</th><th>Disponible</th><th>Accion</th>
            </tr>
          </thead>
          <tbody>
            {filtrados.length === 0 ? (
              <tr><td colSpan='7' className='empty-msg'>No se encontraron recursos</td></tr>
            ) : filtrados.map(r => (
              <tr key={r.id}>
                <td style={{ fontFamily: 'monospace', color: '#083F75' }}>{r.codigo}</td>
                <td style={{ fontWeight: '500' }}>{r.titulo}</td>
                <td>{r.autor}</td>
                <td><span style={{ background: r.tipo === 'Libro' ? '#e3f2fd' : '#fce4ec', color: r.tipo === 'Libro' ? '#0d47a1' : '#880e4f', padding: '3px 10px', borderRadius: '12px', fontSize: '12px' }}>{r.tipo}</span></td>
                <td style={{ textAlign: 'center' }}>{r.stock}</td>
                <td><span className={r.disponible ? 'badge-si' : 'badge-no'}>{r.disponible ? 'Disponible' : 'No disponible'}</span></td>
                <td>
                  <button
                    onClick={() => handleSolicitar(r)}
                    disabled={!r.disponible || loading === r.id}
                    style={{
                      background: r.disponible ? '#083F75' : '#ccc',
                      color: 'white',
                      border: 'none',
                      borderRadius: '6px',
                      padding: '6px 14px',
                      cursor: r.disponible ? 'pointer' : 'not-allowed',
                      fontSize: '13px'
                    }}>
                    {loading === r.id ? 'Solicitando...' : 'Solicitar'}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <p style={{ marginTop: '12px', fontSize: '13px', color: '#888' }}>{filtrados.length} recurso(s) encontrado(s)</p>
    </div>
  );
}

export default Recursos;