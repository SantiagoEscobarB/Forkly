import type { Comensal } from './usuario';

export interface LoginRequest {
    userName: string;
    password: string;
}

export interface RegistroRequest {
    nombreCompleto: string;
    userName: string;
    email: string;
    password: string;
    fechaNacimiento: string; // "yyyy-MM-dd"
}

export interface AuthResponse {
    token: string;
    usuario: Comensal;
}
