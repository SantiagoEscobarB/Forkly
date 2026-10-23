import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import { borrarSesion, EVENTO_SESION_EXPIRADA, guardarSesion, leerSesion } from '../services/sesionStorage';
import type { AuthResponse } from '../types/auth';
import type { Comensal } from '../types/usuario';
import { SesionContext } from './SesionContext';

interface SesionProviderProps {
    children: ReactNode;
}

const SesionProvider = ({ children }: SesionProviderProps) => {
    // Se recupera la sesión guardada para no perderla al recargar la página
    const [sesion, setSesion] = useState<AuthResponse | null>(leerSesion);

    useEffect(() => {
        const alExpirar = () => setSesion(null);
        window.addEventListener(EVENTO_SESION_EXPIRADA, alExpirar);
        return () => window.removeEventListener(EVENTO_SESION_EXPIRADA, alExpirar);
    }, []);

    const iniciarSesion = useCallback((nueva: AuthResponse) => {
        guardarSesion(nueva);
        setSesion(nueva);
    }, []);

    const actualizarUsuario = useCallback((usuario: Comensal) => {
        setSesion((actual) => {
            if (!actual) return actual;
            const nueva = { ...actual, usuario };
            guardarSesion(nueva);
            return nueva;
        });
    }, []);

    const cerrarSesion = useCallback(() => {
        borrarSesion();
        setSesion(null);
    }, []);

    const valor = useMemo(
        () => ({ sesion, iniciarSesion, actualizarUsuario, cerrarSesion }),
        [sesion, iniciarSesion, actualizarUsuario, cerrarSesion],
    );

    return <SesionContext.Provider value={valor}>{children}</SesionContext.Provider>;
};

export default SesionProvider;
