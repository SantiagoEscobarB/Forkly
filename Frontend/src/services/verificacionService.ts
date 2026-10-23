import type { Comensal } from '../types/usuario';
import type { VerificarCorreoRequest } from '../types/verificacion';
import { apiPost } from './api';

// Llamadas a Backend/Controllers/VerificacionController.cs (requieren sesión iniciada)

// Devuelve el comensal con estado VERIFICADA
export const verificarCorreo = (datos: VerificarCorreoRequest) =>
    apiPost<Comensal>('/api/verificacion/confirmar', datos);

export const reenviarCodigo = () =>
    apiPost<void>('/api/verificacion/reenviar');
