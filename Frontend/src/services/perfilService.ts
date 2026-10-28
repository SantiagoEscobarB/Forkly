import type { AvancePerfilRequest, GuardarPerfilRequest, OpcionPerfil, Perfil } from '../types/perfil';
import { apiFetch, apiPost, apiPut } from './api';

// Llamadas a Backend/Controllers/PerfilController.cs (requieren sesión iniciada)

export const obtenerOpciones = () =>
    apiFetch<OpcionPerfil[]>('/api/perfil/opciones');

export const obtenerPerfil = () =>
    apiFetch<Perfil>('/api/perfil');

// Lo elegido hasta ahora en el asistente, para retomarlo si se interrumpe
export const guardarAvance = (datos: AvancePerfilRequest) =>
    apiPut<Perfil>('/api/perfil/avance', datos);

// Confirmar el asistente o guardar desde los ajustes (calcula el vector de preferencias)
export const guardarPerfil = (datos: GuardarPerfilRequest) =>
    apiPut<Perfil>('/api/perfil', datos);

export const omitirPerfil = () =>
    apiPost<Perfil>('/api/perfil/omitir');
