import type { EstadoPerfil } from './usuario';

// Espejo de Backend/Models/DTOs/Perfil

// Backend/Models/Enums/CategoriaOpcion.cs
export type CategoriaOpcion = 'COCINA' | 'AMBIENTE' | 'PRECIO' | 'DIETA' | 'RESTRICCION';

export interface OpcionPerfil {
    codigo: string; // p. ej. "cocina.italiana"
    nombre: string;
    detalle: string | null; // p. ej. el rango en pesos de "$$"
    categoria: CategoriaOpcion;
}

export interface Perfil {
    estado: EstadoPerfil;
    pasoActual: number; // 1 a 4
    seleccionadas: OpcionPerfil[];
    // Un peso por cada tipo de cocina, ambiente y rango de precio; vacío hasta confirmar
    vector: Record<string, number>;
    actualizadoEn: string | null;
}

export interface GuardarPerfilRequest {
    opciones: string[]; // códigos elegidos, de cualquier categoría
}

export interface AvancePerfilRequest extends GuardarPerfilRequest {
    paso: number;
}

export const CATEGORIAS: Record<CategoriaOpcion, { titulo: string; ayuda: string }> = {
    COCINA: { titulo: 'Tipos de cocina', ayuda: '¿Qué te gusta comer? Elige todas las que quieras.' },
    AMBIENTE: { titulo: 'Ambientes', ayuda: '¿En qué tipo de lugar disfrutas más?' },
    PRECIO: { titulo: 'Rango de precio', ayuda: '¿Cuánto sueles gastar por persona?' },
    DIETA: { titulo: 'Tipo de dieta', ayuda: 'Si sigues alguna, elige solo una.' },
    RESTRICCION: { titulo: 'Alergias, intolerancias y otras', ayuda: 'Puedes elegir varias.' },
};

export const ORDEN_CATEGORIAS: CategoriaOpcion[] = ['COCINA', 'AMBIENTE', 'PRECIO', 'DIETA', 'RESTRICCION'];

// Pasos del asistente de H01.09. El último son las restricciones de H01.10.
export const PASOS_PERFIL: CategoriaOpcion[][] = [
    ['COCINA'],
    ['AMBIENTE'],
    ['PRECIO'],
    ['DIETA', 'RESTRICCION'],
];
