import type { AuthResponse, LoginRequest, RegistroRequest } from '../types/auth';
import { apiFetch, borrarToken, guardarToken } from './api';

const guardarSesion = (respuesta: AuthResponse) => {
    guardarToken(respuesta.token);
    return respuesta;
};

export const login = async (datos: LoginRequest) =>
    guardarSesion(await apiFetch<AuthResponse>('/api/auth/login', {
        method: 'POST',
        body: JSON.stringify(datos),
    }));

export const registro = async (datos: RegistroRequest) =>
    guardarSesion(await apiFetch<AuthResponse>('/api/auth/registro', {
        method: 'POST',
        body: JSON.stringify(datos),
    }));

export const logout = borrarToken;
