import { useEffect, useRef, useState } from 'react';
import { cargarGoogle, GOOGLE_CLIENT_ID } from '../services/googleAuth';

// Temporal mientras no exista el Client ID de Google Cloud Console: abre la página de
// Google igual, que mostrará su propio error (falta client_id). Con el Client ID
// configurado se usa el botón oficial de Google Identity Services.
const abrirGoogleSinConfigurar = () => {
    const parametros = new URLSearchParams({
        client_id: '',
        redirect_uri: window.location.origin,
        response_type: 'id_token',
        scope: 'openid email profile',
        nonce: crypto.randomUUID(),
    });
    window.open(`https://accounts.google.com/o/oauth2/v2/auth?${parametros}`, 'google', 'width=500,height=650');
};

interface GoogleButtonProps {
    // Recibe el ID token (credential) que luego se envía al backend
    onCredencial: (idToken: string) => void;
}

const GoogleButton = ({ onCredencial }: GoogleButtonProps) => {
    const contenedor = useRef<HTMLDivElement>(null);
    // Ref para que el callback de Google siempre llame a la versión más reciente
    const onCredencialRef = useRef(onCredencial);
    const [error, setError] = useState('');

    useEffect(() => {
        onCredencialRef.current = onCredencial;
    }, [onCredencial]);

    useEffect(() => {
        const clientId = GOOGLE_CLIENT_ID;
        if (!clientId) return;
        let cancelado = false;

        cargarGoogle()
            .then(() => {
                if (cancelado || !contenedor.current) return;
                google.accounts.id.initialize({
                    client_id: clientId,
                    callback: (respuesta) => onCredencialRef.current(respuesta.credential),
                });
                // Google dibuja su propio botón; el ancho máximo que acepta es 400px
                google.accounts.id.renderButton(contenedor.current, {
                    type: 'standard',
                    theme: 'filled_black',
                    shape: 'pill',
                    size: 'large',
                    text: 'continue_with',
                    locale: 'es',
                    width: Math.min(contenedor.current.offsetWidth, 400),
                });
            })
            .catch(() => setError('No se pudo cargar el inicio de sesión con Google.'));

        return () => {
            cancelado = true;
        };
    }, []);

    return (
        <>
            <div className='flex items-center gap-4 text-sm text-texto-suave'>
                <div className='h-px flex-1 bg-borde' />
                o continúa con
                <div className='h-px flex-1 bg-borde' />
            </div>
            {GOOGLE_CLIENT_ID
                ? <div ref={contenedor} className='flex min-h-11 justify-center' />
                : (
                    <button type='button' onClick={abrirGoogleSinConfigurar}
                        className='rounded-full border border-borde bg-panel py-3 font-semibold transition-colors hover:border-primary'>
                        Continuar con Google
                    </button>
                )}
            {error && <p role='alert' className='text-center text-sm text-red-400'>{error}</p>}
        </>
    );
};

export default GoogleButton;
