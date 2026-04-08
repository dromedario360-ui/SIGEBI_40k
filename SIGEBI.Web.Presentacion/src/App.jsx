import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Recursos from './pages/Recursos';
import Prestamos from './pages/Prestamos';
import Penalizaciones from './pages/Penalizaciones';
import './index.css';
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<Login />} />
        <Route path='/recursos' element={<><Navbar /><Recursos /></>} />
        <Route path='/prestamos' element={<><Navbar /><Prestamos /></>} />
        <Route path='/penalizaciones' element={<><Navbar /><Penalizaciones /></>} />
      </Routes>
    </BrowserRouter>
  );
}
export default App;
