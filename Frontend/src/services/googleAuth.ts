export const GOOGLE_CLIENT_ID = import.meta.env.VITE_GOOGLE_CLIENT_ID;

let cargando: Promise<void> | null = null;

// Carga una sola vez el script de Google Identity Services
export const cargarGoogle = () => {
    cargando ??= new Promise<void>((resolve, reject) => {
        const script = document.createElement('script');
        script.src = 'https://accounts.google.com/gsi/client';
        script.async = true;
        script.onload = () => resolve();
        script.onerror = () => {
            cargando = null;
            reject(new Error('No se pudo cargar Google.'));
        };
        document.head.appendChild(script);
    });
    return cargando;
};
