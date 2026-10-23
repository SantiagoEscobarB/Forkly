import type { ReactNode } from 'react';
import { Navigate } from 'react-router';
import { useSesion } from '../hooks/useSesion';
import { rutaSegunSesion, type Ruta } from './rutas';

interface RutaProtegidaProps {
    ruta: Ruta;
    children: ReactNode;
}

// Muestra la página solo si corresponde al estado de la sesión; si no, redirige a la que sí.
// Por eso, al iniciar sesión o verificar el correo, la navegación ocurre sola.
const RutaProtegida = ({ ruta, children }: RutaProtegidaProps) => {
    const { sesion } = useSesion();
    const destino = rutaSegunSesion(sesion);

    return destino === ruta ? children : <Navigate to={destino} replace />;
};

export default RutaProtegida;
