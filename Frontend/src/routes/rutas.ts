import type { AuthResponse } from '../types/auth';

export const RUTAS = {
    auth: '/auth',
    verificarCorreo: '/verificar-correo',
    inicio: '/',
    // Públicas: no dependen del estado de la sesión (AppRoutes)
    recuperarContrasena: '/recuperar-contrasena',
    restablecerContrasena: '/restablecer-contrasena', // el enlace del correo apunta aquí
} as const;

export type Ruta = typeof RUTAS[keyof typeof RUTAS];

// Cada estado de sesión tiene una sola página válida:
// sin sesión -> auth, correo sin verificar -> verificar-correo, verificado -> inicio
export const rutaSegunSesion = (sesion: AuthResponse | null): Ruta => {
    if (!sesion) return RUTAS.auth;
    if (sesion.usuario.estado === 'NO_VERIFICADA') return RUTAS.verificarCorreo;
    return RUTAS.inicio;
};
