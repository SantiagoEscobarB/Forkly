import AuthLayout from '../components/layout/AuthLayout';
import VerificacionForm from '../components/verificacion/VerificacionForm';
import { useSesion } from '../hooks/useSesion';

// Al verificar, el estado pasa a VERIFICADA y RutaProtegida lleva al inicio
const VerificacionPage = () => {
    const { sesion, actualizarUsuario, cerrarSesion } = useSesion();

    return (
        <AuthLayout titulo='Verifica tu correo'
            subtitulo={<>Ingresa el código de 6 dígitos que enviamos a{' '}
                <span className='font-semibold text-white'>{sesion?.usuario.email}</span>.</>}>
            <VerificacionForm onVerificado={actualizarUsuario} />
            <button type='button' onClick={cerrarSesion} className='text-sm text-texto-suave hover:text-white'>
                ¿No es tu correo? Cerrar sesión
            </button>
        </AuthLayout>
    );
};

export default VerificacionPage;
