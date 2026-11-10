import { Navigate, Route, Routes } from 'react-router';
import AuthPage from '../pages/AuthPage';
import ConfigurarPerfilPage from '../pages/ConfigurarPerfilPage';
import FichaRestaurantePage from '../pages/FichaRestaurantePage';
import InicioPage from '../pages/InicioPage';
import PerfilGastronomicoPage from '../pages/PerfilGastronomicoPage';
import RecuperarContrasenaPage from '../pages/RecuperarContrasenaPage';
import RestaurantesPage from '../pages/RestaurantesPage';
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
            <Route path={RUTAS.configurarPerfil}
                element={<RutaProtegida ruta={RUTAS.configurarPerfil}><ConfigurarPerfilPage /></RutaProtegida>} />
            <Route path={RUTAS.inicio} element={<RutaProtegida ruta={RUTAS.inicio}><InicioPage /></RutaProtegida>} />
            <Route path={RUTAS.perfilGastronomico}
                element={<RutaProtegida ruta={RUTAS.perfilGastronomico}><PerfilGastronomicoPage /></RutaProtegida>} />
            <Route path={RUTAS.restaurantes}
                element={<RutaProtegida ruta={RUTAS.restaurantes}><RestaurantesPage /></RutaProtegida>} />
            <Route path={RUTAS.fichaRestaurante}
                element={<RutaProtegida ruta={RUTAS.fichaRestaurante}><FichaRestaurantePage /></RutaProtegida>} />
            {/* Sin RutaProtegida: el enlace del correo se puede abrir con o sin sesión */}
            <Route path={RUTAS.recuperarContrasena} element={<RecuperarContrasenaPage />} />
            <Route path={RUTAS.restablecerContrasena} element={<RestablecerContrasenaPage />} />
            <Route path='*' element={<Navigate to={RUTAS.inicio} replace />} />
        </Routes>
    );
};

export default AppRoutes;
