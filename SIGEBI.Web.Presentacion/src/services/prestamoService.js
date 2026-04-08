import api from './api';
export const getPrestamosPorUsuario = (id) => api.get('/Prestamos/usuario/' + id);
export const crearPrestamo = (data) => api.post('/Prestamos', data);
export const procesarDevolucion = (id) => api.patch('/Prestamos/' + id + '/devolver');
