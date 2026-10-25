import { useState } from 'react';
import { Link } from 'react-router';
import AuthLayout from '../components/layout/AuthLayout';
import SolicitarRecuperacionForm from '../components/recuperacion/SolicitarRecuperacionForm';
import { RUTAS } from '../routes/rutas';
import { MINUTOS_VIGENCIA_ENLACE } from '../types/recuperacion';

// H01.06: pide el correo y envía el enlace para restablecer la contraseña.
// El mensaje de confirmación es el mismo exista o no la cuenta.
const RecuperarContrasenaPage = () => {
    // Correo al que se pidió el enlace; null mientras se muestra el formulario
    const [enviadoA, setEnviadoA] = useState<string | null>(null);
    const [ultimoCorreo, setUltimoCorreo] = useState('');

    const alEnviar = (email: string) => {
        setUltimoCorreo(email);
        setEnviadoA(email);
    };

    return (
        <AuthLayout
            titulo={enviadoA ? 'Revisa tu correo' : 'Recupera tu contraseña'}
            subtitulo={enviadoA
                ? <>Si <span className='font-semibold text-white'>{enviadoA}</span> tiene una cuenta en Forkly,
                    te enviamos un enlace para crear una contraseña nueva.</>
                : 'Escribe el correo de tu cuenta y te enviaremos un enlace para crear una contraseña nueva.'}>
            {enviadoA ? (
                <div className='flex flex-col gap-6'>
                    <p role='status' className='rounded-2xl border border-borde bg-panel px-5 py-4 text-sm text-texto-suave'>
                        El enlace sirve una sola vez y vence en {MINUTOS_VIGENCIA_ENLACE} minutos.
                        Revisa también la carpeta de spam.
                    </p>
                    <button type='button' onClick={() => setEnviadoA(null)}
                        className='rounded-full border border-borde bg-panel py-3 font-semibold transition-colors hover:border-primary'>
                        Enviar de nuevo o usar otro correo
                    </button>
                </div>
            ) : (
                <SolicitarRecuperacionForm emailInicial={ultimoCorreo} onEnviado={alEnviar} />
            )}
            <Link to={RUTAS.auth} className='text-center text-sm text-texto-suave hover:text-white'>
                Volver a iniciar sesión
            </Link>
        </AuthLayout>
    );
};

export default RecuperarContrasenaPage;
