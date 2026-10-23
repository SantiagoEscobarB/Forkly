import type { AuthResponse } from '../types/auth';

// Único lugar que lee y escribe la sesión en el navegador
const SESION_KEY = 'forkly_sesion';
const CODIGO_ENVIADO_KEY = 'forkly_codigo_enviado';

// Evento que avisa al SesionProvider cuando la API rechaza el token (401)
export const EVENTO_SESION_EXPIRADA = 'forkly:sesion-expirada';

export const leerSesion = (): AuthResponse | null => {
    try {
        const guardada = localStorage.getItem(SESION_KEY);
        return guardada ? JSON.parse(guardada) as AuthResponse : null;
    } catch {
        return null;
    }
};

export const guardarSesion = (sesion: AuthResponse) =>
    localStorage.setItem(SESION_KEY, JSON.stringify(sesion));

export const borrarSesion = () => {
    localStorage.removeItem(SESION_KEY);
    sessionStorage.removeItem(CODIGO_ENVIADO_KEY);
};

export const leerToken = () => leerSesion()?.token ?? null;

// Momento del último envío del código, para que la espera antes de reenviar
// se mantenga aunque se recargue la página
export const marcarCodigoEnviado = () =>
    sessionStorage.setItem(CODIGO_ENVIADO_KEY, String(Date.now()));

export const segundosDesdeCodigoEnviado = () => {
    const enviado = Number(sessionStorage.getItem(CODIGO_ENVIADO_KEY));
    return enviado ? Math.floor((Date.now() - enviado) / 1000) : Infinity;
};
