// Espejo de Backend/Models/DTOs/Recuperacion

export interface SolicitarRecuperacionRequest {
    email: string;
}

// Los dos parámetros del enlace del correo: /restablecer-contrasena?id=...&token=...
export interface EnlaceRecuperacion {
    usuarioId: string;
    token: string;
}

export interface RestablecerContrasenaRequest extends EnlaceRecuperacion {
    nuevaContrasena: string;
}

// Backend/Service/Auth/PoliticaAcceso.cs
export const MINUTOS_VIGENCIA_ENLACE = 30;
