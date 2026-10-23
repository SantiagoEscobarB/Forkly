import { createContext } from 'react';
import type { AuthResponse } from '../types/auth';
import type { Comensal } from '../types/usuario';

export interface SesionContextValue {
    sesion: AuthResponse | null;
    iniciarSesion: (sesion: AuthResponse) => void;
    // Reemplaza los datos del usuario conservando el token (p. ej. tras verificar el correo)
    actualizarUsuario: (usuario: Comensal) => void;
    cerrarSesion: () => void;
}

export const SesionContext = createContext<SesionContextValue | null>(null);
