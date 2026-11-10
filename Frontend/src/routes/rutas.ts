import type { AuthResponse } from '../types/auth';

export const RUTAS = {
    auth: '/auth',
    verificarCorreo: '/verificar-correo',
    configurarPerfil: '/configurar-perfil',
    inicio: '/',
    perfilGastronomico: '/perfil-gastronomico', // ajustes del perfil (H01.09 y H01.10)
    restaurantes: '/restaurantes', // búsqueda de restaurantes (FOR-62)
    fichaRestaurante: '/restaurantes/:id', // ficha de un restaurante (FOR-63)
    // Públicas: no dependen del estado de la sesión (AppRoutes)
    recuperarContrasena: '/recuperar-contrasena',
    restablecerContrasena: '/restablecer-contrasena', // el enlace del correo apunta aquí
} as const;

export type Ruta = typeof RUTAS[keyof typeof RUTAS];

// Cada estado de sesión tiene una página principal:
// sin sesión -> auth, correo sin verificar -> verificar-correo,
// perfil gastronómico sin configurar -> configurar-perfil (H01.09), lo demás -> inicio
export const rutaSegunSesion = (sesion: AuthResponse | null): Ruta => {
    if (!sesion) return RUTAS.auth;
    if (sesion.usuario.estado === 'NO_VERIFICADA') return RUTAS.verificarCorreo;
    const { estadoPerfil } = sesion.usuario;
    if (estadoPerfil === 'PENDIENTE' || estadoPerfil === 'EN_PROGRESO') return RUTAS.configurarPerfil;
    return RUTAS.inicio;
};

// Páginas a las que se puede entrar desde el inicio, además del inicio mismo
// (FOR-62, criterio 2, y FOR-63, criterio 1: los mismos casos que "Mi perfil gastronómico")
const RUTAS_DESDE_INICIO: readonly Ruta[] = [
    RUTAS.inicio,
    RUTAS.perfilGastronomico,
    RUTAS.restaurantes,
    RUTAS.fichaRestaurante,
];

// Dirección de la ficha de un restaurante, p. ej. /restaurantes/7
export const rutaFichaRestaurante = (id: number) => `/restaurantes/${id}`;

export const puedeVerRuta = (ruta: Ruta, sesion: AuthResponse | null) => {
    const destino = rutaSegunSesion(sesion);
    return ruta === destino || (destino === RUTAS.inicio && RUTAS_DESDE_INICIO.includes(ruta));
};
