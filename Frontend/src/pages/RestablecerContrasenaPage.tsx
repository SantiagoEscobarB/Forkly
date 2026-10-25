import { useEffect, useState } from 'react';
import { Link, useSearchParams } from 'react-router';
import AuthLayout from '../components/layout/AuthLayout';
import RestablecerContrasenaForm from '../components/recuperacion/RestablecerContrasenaForm';
import ErrorMessage from '../components/ui/ErrorMessage';
import { useSesion } from '../hooks/useSesion';
import { RUTAS } from '../routes/rutas';
import { ApiError, mensajesDeError } from '../services/api';
import { STATUS_ENLACE_INVALIDO, validarEnlace } from '../services/recuperacionService';
import { MINUTOS_VIGENCIA_ENLACE } from '../types/recuperacion';

// Enlaces con aspecto de botón principal (mismas clases que SubmitButton)
const CLASES_BOTON = 'block w-full rounded-full bg-primary py-4 text-center text-lg font-semibold text-fondo '
    + 'transition-colors hover:bg-primary-hover';

type Estado = 'validando' | 'valido' | 'invalido' | 'error' | 'restablecida';

const textos: Record<Estado, { titulo: string; subtitulo: string }> = {
    validando: { titulo: 'Crea tu contraseña nueva', subtitulo: 'Estamos revisando el enlace...' },
    valido: { titulo: 'Crea tu contraseña nueva', subtitulo: 'Debe cumplir las mismas reglas que al crear tu cuenta.' },
    invalido: {
        titulo: 'El enlace ya no sirve',
        subtitulo: `Este enlace ya se usó o venció. Cada enlace sirve una sola vez y dura ${MINUTOS_VIGENCIA_ENLACE} minutos.`,
    },
    error: { titulo: 'Crea tu contraseña nueva', subtitulo: 'No pudimos revisar el enlace.' },
    restablecida: {
        titulo: 'Contraseña actualizada',
        subtitulo: 'Por seguridad cerramos todas las sesiones que tenías abiertas. Inicia sesión con tu contraseña nueva.',
    },
};

// H01.06: página a la que lleva el enlace del correo (/restablecer-contrasena?id=...&token=...)
const RestablecerContrasenaPage = () => {
    const [parametros] = useSearchParams();
    const usuarioId = parametros.get('id') ?? '';
    const token = parametros.get('token') ?? '';
    const { sesion, cerrarSesion } = useSesion();

    const [estado, setEstado] = useState<Estado>(usuarioId && token ? 'validando' : 'invalido');
    const [errores, setErrores] = useState<string[]>([]);

    // Se revisa el enlace al abrirlo para no hacer escribir la contraseña en vano
    useEffect(() => {
        if (!usuarioId || !token) return;
        let cancelado = false;
        validarEnlace({ usuarioId, token })
            .then(() => {
                if (!cancelado) setEstado('valido');
            })
            .catch((error: unknown) => {
                if (cancelado) return;
                if (error instanceof ApiError && error.status === STATUS_ENLACE_INVALIDO) {
                    setEstado('invalido');
                } else {
                    setErrores(mensajesDeError(error));
                    setEstado('error');
                }
            });
        return () => {
            cancelado = true;
        };
    }, [usuarioId, token]);

    const alRestablecer = () => {
        // El backend ya invalidó todos los tokens; también se borra la sesión de este navegador
        if (sesion) cerrarSesion();
        setEstado('restablecida');
    };

    const { titulo, subtitulo } = textos[estado];

    return (
        <AuthLayout titulo={titulo} subtitulo={subtitulo}>
            {estado === 'valido' && (
                <RestablecerContrasenaForm enlace={{ usuarioId, token }}
                    onRestablecida={alRestablecer} onEnlaceInvalido={() => setEstado('invalido')} />
            )}
            {estado === 'invalido' && (
                <Link to={RUTAS.recuperarContrasena} className={CLASES_BOTON}>Solicitar un enlace nuevo</Link>
            )}
            {estado === 'error' && (
                <>
                    <ErrorMessage errores={errores} />
                    <button type='button' onClick={() => window.location.reload()}
                        className='rounded-full border border-borde bg-panel py-3 font-semibold transition-colors hover:border-primary'>
                        Intentar de nuevo
                    </button>
                </>
            )}
            {estado === 'restablecida' && (
                <Link to={RUTAS.auth} className={CLASES_BOTON}>Iniciar sesión</Link>
            )}
            {estado !== 'restablecida' && (
                <Link to={RUTAS.auth} className='text-center text-sm text-texto-suave hover:text-white'>
                    Volver a iniciar sesión
                </Link>
            )}
        </AuthLayout>
    );
};

export default RestablecerContrasenaPage;
