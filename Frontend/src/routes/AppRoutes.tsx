import { Navigate, Route, Routes } from 'react-router';
import AuthPage from '../pages/AuthPage';
import InicioPage from '../pages/InicioPage';
import RecuperarContrasenaPage from '../pages/RecuperarContrasenaPage';
import RestablecerContrasenaPage from '../pages/RestablecerContrasenaPage';
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
            {/* Sin RutaProtegida: el enlace del correo se puede abrir con o sin sesión */}
            <Route path={RUTAS.recuperarContrasena} element={<RecuperarContrasenaPage />} />
            <Route path={RUTAS.restablecerContrasena} element={<RestablecerContrasenaPage />} />
            <Route path='*' element={<Navigate to={RUTAS.inicio} replace />} />
        </Routes>
    );
};

export default AppRoutes;
