import api from './api';
export const getPenalizacionesPorUsuario = (id) => api.get('/Penalizaciones/usuario/' + id);
