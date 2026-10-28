import type { ReactNode } from 'react';
import { Navigate } from 'react-router';
import { useSesion } from '../hooks/useSesion';
import { puedeVerRuta, rutaSegunSesion, type Ruta } from './rutas';

interface RutaProtegidaProps {
    ruta: Ruta;
    children: ReactNode;
}

// Muestra la página solo si corresponde al estado de la sesión; si no, redirige a la que sí.
// Por eso, al iniciar sesión, verificar el correo o terminar el perfil, la navegación ocurre sola.
const RutaProtegida = ({ ruta, children }: RutaProtegidaProps) => {
    const { sesion } = useSesion();

    return puedeVerRuta(ruta, sesion) ? children : <Navigate to={rutaSegunSesion(sesion)} replace />;
};

export default RutaProtegida;
