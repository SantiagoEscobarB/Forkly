import { borrarSesion, EVENTO_SESION_EXPIRADA, leerToken } from './sesionStorage';

// status 0 = no hubo respuesta (red caída, backend apagado)
export class ApiError extends Error {
    status: number;
    cuerpo: unknown;

    constructor(status: number, mensaje: string, cuerpo?: unknown) {
        super(mensaje);
        this.status = status;
        this.cuerpo = cuerpo;
    }
}

const leerCuerpo = async (respuesta: Response): Promise<unknown> => {
    const texto = await respuesta.text();
    if (!texto) return undefined;
    try {
        return JSON.parse(texto);
    } catch {
        return texto;
    }
};

export const apiFetch = async <T>(ruta: string, opciones: RequestInit = {}): Promise<T> => {
    const token = leerToken();
    const headers = new Headers(opciones.headers);
    if (opciones.body && !(opciones.body instanceof FormData) && !headers.has('Content-Type'))
        headers.set('Content-Type', 'application/json');
    if (token) headers.set('Authorization', `Bearer ${token}`);

    let respuesta: Response;
    try {
        respuesta = await fetch(ruta, { ...opciones, headers });
    } catch {
        throw new ApiError(0, 'No se pudo conectar con el servidor.');
    }

    const cuerpo = await leerCuerpo(respuesta);

    if (!respuesta.ok) {
        // Token vencido o inválido: se cierra la sesión y se avisa al SesionProvider
        if (respuesta.status === 401 && token) {
            borrarSesion();
            window.dispatchEvent(new Event(EVENTO_SESION_EXPIRADA));
        }
        const mensaje = (cuerpo as { message?: string } | undefined)?.message ?? `Error ${respuesta.status}`;
        throw new ApiError(respuesta.status, mensaje, cuerpo);
    }

    return cuerpo as T;
};

export const apiPost = <T>(ruta: string, datos?: unknown) =>
    apiFetch<T>(ruta, { method: 'POST', body: datos === undefined ? undefined : JSON.stringify(datos) });

export const apiPut = <T>(ruta: string, datos?: unknown) =>
    apiFetch<T>(ruta, { method: 'PUT', body: datos === undefined ? undefined : JSON.stringify(datos) });

const MENSAJE_GENERAL = 'Ocurrió un error. Intenta de nuevo más tarde.';

// Convierte cualquier error en una lista de mensajes para mostrar.
// `mensajesPorStatus` permite a cada formulario dar su propio texto para un status.
export const mensajesDeError = (error: unknown, mensajesPorStatus: Record<number, string> = {}): string[] => {
    if (!(error instanceof ApiError)) return [MENSAJE_GENERAL];
    if (error.status === 0) return [error.message];
    if (mensajesPorStatus[error.status]) return [mensajesPorStatus[error.status]];

    const cuerpo = error.cuerpo as {
        errores?: string[];
        errors?: Record<string, string[]>;
        message?: string;
    } | undefined;

    // Errores de Identity (AuthController) o de validación de ASP.NET Core (ValidationProblem)
    if (cuerpo?.errores?.length) return cuerpo.errores;
    if (cuerpo?.errors) return Object.values(cuerpo.errors).flat();
    if (error.status >= 400 && error.status < 500 && cuerpo?.message) return [cuerpo.message];
    return [MENSAJE_GENERAL];
};
