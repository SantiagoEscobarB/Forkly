import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import { renovarSesion } from '../services/authService';
import { borrarSesion, EVENTO_SESION_EXPIRADA, guardarSesion, leerSesion, leerToken } from '../services/sesionStorage';
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

    // Al abrir la app se renueva el token y se traen los datos actuales del usuario, así la
    // sesión se mantiene sin volver a pedir las credenciales (H01.04). Si el backend rechaza
    // el token (venció o se restableció la contraseña), apiFetch borra la sesión y dispara
    // EVENTO_SESION_EXPIRADA. Sin conexión se conserva la sesión guardada.
    useEffect(() => {
        if (!leerToken()) return;
        let cancelado = false;
        renovarSesion()
            .then((renovada) => {
                if (cancelado) return;
                guardarSesion(renovada);
                setSesion(renovada);
            })
            .catch(() => { /* 401 ya lo maneja apiFetch; otros errores conservan la sesión */ });
        return () => {
            cancelado = true;
        };
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
