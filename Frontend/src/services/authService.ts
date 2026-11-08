import type { AuthResponse, LoginRequest, RegistroRequest } from '../types/auth';
import { apiFetch, apiPost } from './api';

// Llamadas a Backend/Controllers/AuthController.cs. Guardar la sesión le
// corresponde a SesionProvider (iniciarSesion).

export const login = (datos: LoginRequest) =>
    apiPost<AuthResponse>('/api/auth/login', datos);

// Al abrir la app con una sesión guardada: token nuevo y datos actuales del usuario.
// Si el token ya no sirve responde 401 y apiFetch cierra la sesión.
export const renovarSesion = () =>
    apiFetch<AuthResponse>('/api/auth/sesion');

export const registro = (datos: RegistroRequest) =>
    apiPost<AuthResponse>('/api/auth/registro', datos);
