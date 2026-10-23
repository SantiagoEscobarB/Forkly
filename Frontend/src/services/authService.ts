import type { AuthResponse, GoogleAuthResponse, GoogleRegistroRequest, LoginRequest, RegistroRequest } from '../types/auth';
import { apiFetch, borrarToken, guardarToken } from './api';

const guardarSesion = (respuesta: AuthResponse) => {
    guardarToken(respuesta.token);
    return respuesta;
};

const post = <T>(ruta: string, datos: unknown) =>
    apiFetch<T>(ruta, { method: 'POST', body: JSON.stringify(datos) });

export const login = async (datos: LoginRequest) =>
    guardarSesion(await post<AuthResponse>('/api/auth/login', datos));

export const registro = async (datos: RegistroRequest) =>
    guardarSesion(await post<AuthResponse>('/api/auth/registro', datos));

export const loginGoogle = async (idToken: string) => {
    const respuesta = await post<GoogleAuthResponse>('/api/auth/google', { idToken });
    if (!respuesta.requiereRegistro) guardarSesion(respuesta.sesion);
    return respuesta;
};

export const registroGoogle = async (datos: GoogleRegistroRequest) =>
    guardarSesion(await post<AuthResponse>('/api/auth/google/registro', datos));

export const logout = borrarToken;
