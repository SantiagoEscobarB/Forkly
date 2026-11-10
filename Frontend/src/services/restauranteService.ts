import type { FichaRestaurante, RestauranteResumen } from '../types/restaurante';
import { apiFetch } from './api';

// Llamadas a Backend/Controllers/RestaurantesController.cs (requieren sesión iniciada)

export const obtenerRestaurantes = () =>
    apiFetch<RestauranteResumen[]>('/api/restaurantes');

// El id es el texto de la dirección /restaurantes/{id}; si no es un Id, el backend responde 404
export const obtenerFichaRestaurante = (id: string) =>
    apiFetch<FichaRestaurante>(`/api/restaurantes/${encodeURIComponent(id)}`);
