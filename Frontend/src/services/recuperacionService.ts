import type { EnlaceRecuperacion, RestablecerContrasenaRequest, SolicitarRecuperacionRequest } from '../types/recuperacion';
import { apiPost } from './api';

// Llamadas a Backend/Controllers/RecuperacionController.cs (no requieren sesión)

// Status con el que el backend responde a un enlace inválido, usado o vencido
export const STATUS_ENLACE_INVALIDO = 410;

// Responde igual exista o no el correo
export const solicitarRecuperacion = (datos: SolicitarRecuperacionRequest) =>
    apiPost<void>('/api/recuperacion/solicitar', datos);

export const validarEnlace = (datos: EnlaceRecuperacion) =>
    apiPost<void>('/api/recuperacion/validar', datos);

export const restablecerContrasena = (datos: RestablecerContrasenaRequest) =>
    apiPost<void>('/api/recuperacion/restablecer', datos);
