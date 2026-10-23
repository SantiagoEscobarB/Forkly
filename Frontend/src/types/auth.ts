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

export interface GoogleRegistroRequest {
    idToken: string;
    nombreCompleto: string;
    userName: string;
    fechaNacimiento: string; // "yyyy-MM-dd"
}

// Backend/Models/DTOs/GoogleAuthResponseDTO.cs
export type GoogleAuthResponse =
    | { requiereRegistro: false; sesion: AuthResponse }
    | { requiereRegistro: true; nombreCompleto: string | null; email: string };
