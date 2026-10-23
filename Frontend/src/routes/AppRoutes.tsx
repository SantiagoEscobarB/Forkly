import { Navigate, Route, Routes } from 'react-router';
import AuthPage from '../pages/AuthPage';
import InicioPage from '../pages/InicioPage';
import VerificacionPage from '../pages/VerificacionPage';
import RutaProtegida from './RutaProtegida';
import { RUTAS } from './rutas';

const AppRoutes = () => {
    return (
        <Routes>
            <Route path={RUTAS.auth} element={<RutaProtegida ruta={RUTAS.auth}><AuthPage /></RutaProtegida>} />
            <Route path={RUTAS.verificarCorreo}
                element={<RutaProtegida ruta={RUTAS.verificarCorreo}><VerificacionPage /></RutaProtegida>} />
            <Route path={RUTAS.inicio} element={<RutaProtegida ruta={RUTAS.inicio}><InicioPage /></RutaProtegida>} />
            <Route path='*' element={<Navigate to={RUTAS.inicio} replace />} />
        </Routes>
    );
};

export default AppRoutes;
