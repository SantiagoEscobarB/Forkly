import type { CategoriaOpcion, OpcionPerfil } from '../types/perfil';

export const opcionesDe = (opciones: OpcionPerfil[], categoria: CategoriaOpcion) =>
    opciones.filter((opcion) => opcion.categoria === categoria);

// Agrega el código si no estaba elegido y lo quita si ya lo estaba
export const alternar = (seleccion: string[], codigo: string) =>
    seleccion.includes(codigo) ? seleccion.filter((c) => c !== codigo) : [...seleccion, codigo];

// H01.10, criterio 2: solo se puede elegir un tipo de dieta (el backend también lo valida)
export const hayVariasDietas = (seleccion: string[], opciones: OpcionPerfil[]) =>
    opcionesDe(opciones, 'DIETA').filter((opcion) => seleccion.includes(opcion.codigo)).length > 1;
