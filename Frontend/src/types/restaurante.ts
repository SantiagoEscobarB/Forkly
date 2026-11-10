// Espejo de Backend/Models/DTOs/Restaurantes

// Cada restaurante de GET /api/restaurantes (FOR-62, criterio 9)
export interface RestauranteResumen {
    id: number;
    nombre: string;
    barrio: string;
    tiposCocina: string[]; // nombres, en el orden del catálogo
    rangoPrecio: string; // p. ej. "De $25.000 a $49.999"
}

// La ficha de GET /api/restaurantes/{id} (FOR-63, criterio 8)
export interface FichaRestaurante {
    nombre: string;
    rangoPrecio: string;
    direccion: string;
    barrio: string;
    telefono: string; // p. ej. "604 444 1234"
    tiposCocina: string[]; // nombres, en el orden del catálogo
    ambientes: string[]; // nombres, en el orden del catálogo
    fuenteUrl: string;
}
