import type { RestauranteResumen } from '../types/restaurante';

// FOR-62, criterio 6: la búsqueda no distingue mayúsculas ni las tildes á, é, í, ó, ú, ü,
// pero sí distingue la ñ de la n (por eso no se usa normalize('NFD'), que convierte la ñ en n)
const SIN_TILDE: Record<string, string> = { á: 'a', é: 'e', í: 'i', ó: 'o', ú: 'u', ü: 'u' };

// NFC une la letra y su tilde si llegan separadas (p. ej. "e" + "´" se vuelve "é")
export const normalizarBusqueda = (texto: string) =>
    texto.normalize('NFC').toLowerCase().replace(/[áéíóúü]/g, (letra) => SIN_TILDE[letra]);

// FOR-62, criterio 4: orden alfabético por nombre, sin distinguir mayúsculas ni tildes
const comparador = new Intl.Collator('es', { sensitivity: 'base' });

export const ordenarPorNombre = (restaurantes: RestauranteResumen[]) =>
    [...restaurantes].sort((a, b) => comparador.compare(a.nombre, b.nombre));

// FOR-62, criterios 6 y 7: el nombre contiene el texto escrito (sin los espacios del inicio y
// del final) y, si hay tipos de cocina elegidos, el restaurante tiene al menos uno de ellos
export const filtrarRestaurantes = (restaurantes: RestauranteResumen[], texto: string, cocinas: string[]) => {
    const buscado = normalizarBusqueda(texto.trim());
    return restaurantes.filter((restaurante) =>
        normalizarBusqueda(restaurante.nombre).includes(buscado)
        && (cocinas.length === 0 || restaurante.tiposCocina.some((cocina) => cocinas.includes(cocina))));
};

// FOR-62, criterio 5
export const textoContador = (cantidad: number) =>
    cantidad === 1 ? '1 restaurante' : `${cantidad} restaurantes`;
