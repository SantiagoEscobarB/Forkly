import { useState } from 'react';
import AuthTabs, { type Modo } from '../components/auth/AuthTabs';
import GoogleButton from '../components/auth/GoogleButton';
import GoogleRegistroForm, { type GooglePendiente } from '../components/auth/GoogleRegistroForm';
import LoginForm from '../components/auth/LoginForm';
import RegisterForm from '../components/auth/RegisterForm';
import AuthLayout from '../components/layout/AuthLayout';
import ErrorMessage from '../components/ui/ErrorMessage';
import { useSesion } from '../hooks/useSesion';
import { mensajesDeError } from '../services/api';
import { loginGoogle } from '../services/authService';
import { marcarCodigoEnviado } from '../services/sesionStorage';
import type { AuthResponse } from '../types/auth';

// Al iniciar sesión, RutaProtegida lleva al usuario a verificar su correo o al inicio
const AuthPage = () => {
    const { iniciarSesion } = useSesion();
    const [modo, setModo] = useState<Modo>('login');
    // Cuenta de Google que todavía no existe en Forkly y debe completar su perfil
    const [googlePendiente, setGooglePendiente] = useState<GooglePendiente | null>(null);
    const [erroresGoogle, setErroresGoogle] = useState<string[]>([]);

    const alRegistrarse = (respuesta: AuthResponse) => {
        // El backend acaba de enviar el código al correo
        if (respuesta.usuario.estado === 'NO_VERIFICADA') marcarCodigoEnviado();
        iniciarSesion(respuesta);
    };

    const entrarConGoogle = async (idToken: string) => {
        setErroresGoogle([]);
        try {
            const respuesta = await loginGoogle(idToken);
            if (respuesta.requiereRegistro)
                setGooglePendiente({ idToken, nombreCompleto: respuesta.nombreCompleto ?? '', email: respuesta.email });
            else
                iniciarSesion(respuesta.sesion);
        } catch (error) {
            setErroresGoogle(mensajesDeError(error));
        }
    };

    const titulo = googlePendiente ? 'Completa tu perfil'
        : modo === 'login' ? 'Bienvenido de nuevo' : 'Crea tu cuenta';
    const subtitulo = googlePendiente ? 'Solo nos faltan unos datos.'
        : modo === 'login' ? 'Inicia sesión para continuar.' : 'Únete a la comunidad de Forkly.';

    return (
        <AuthLayout titulo={titulo} subtitulo={subtitulo}>
            {googlePendiente && (
                <GoogleRegistroForm pendiente={googlePendiente} onExito={alRegistrarse}
                    onCancelar={() => setGooglePendiente(null)} />
            )}
            {/* Se oculta en vez de desmontarse para no perder lo escrito ni el botón de Google */}
            <div className='flex flex-col gap-8' hidden={googlePendiente !== null}>
                <AuthTabs modo={modo} setModo={setModo} />
                {/* Ambos formularios quedan montados para no perder lo escrito al cambiar de pestaña */}
                <div id='panel-login' role='tabpanel' aria-labelledby='tab-login' hidden={modo !== 'login'}>
                    <LoginForm onExito={iniciarSesion} />
                </div>
                <div id='panel-registro' role='tabpanel' aria-labelledby='tab-registro' hidden={modo !== 'registro'}>
                    <RegisterForm onExito={alRegistrarse} />
                </div>
                <GoogleButton onCredencial={entrarConGoogle} />
                <ErrorMessage errores={erroresGoogle} />
            </div>
        </AuthLayout>
    );
};

export default AuthPage;
