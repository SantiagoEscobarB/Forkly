import type { AuthResponse, GoogleAuthResponse, GoogleRegistroRequest, LoginRequest, RegistroRequest } from '../types/auth';
import { apiPost } from './api';

// Llamadas a Backend/Controllers/AuthController.cs. Guardar la sesión le
// corresponde a SesionProvider (iniciarSesion).

export const login = (datos: LoginRequest) =>
    apiPost<AuthResponse>('/api/auth/login', datos);

export const registro = (datos: RegistroRequest) =>
    apiPost<AuthResponse>('/api/auth/registro', datos);

export const loginGoogle = (idToken: string) =>
    apiPost<GoogleAuthResponse>('/api/auth/google', { idToken });

export const registroGoogle = (datos: GoogleRegistroRequest) =>
    apiPost<AuthResponse>('/api/auth/google/registro', datos);
