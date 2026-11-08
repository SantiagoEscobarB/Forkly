import { useState } from 'react';
import AuthTabs, { type Modo } from '../components/auth/AuthTabs';
import LoginForm from '../components/auth/LoginForm';
import RegisterForm from '../components/auth/RegisterForm';
import AuthLayout from '../components/layout/AuthLayout';
import { useSesion } from '../hooks/useSesion';
import { marcarCodigoEnviado } from '../services/sesionStorage';
import type { AuthResponse } from '../types/auth';

// Al iniciar sesión, RutaProtegida lleva al usuario a verificar su correo o al inicio
const AuthPage = () => {
    const { iniciarSesion } = useSesion();
    const [modo, setModo] = useState<Modo>('login');

    const alRegistrarse = (respuesta: AuthResponse) => {
        // El backend acaba de enviar el código al correo
        if (respuesta.usuario.estado === 'NO_VERIFICADA') marcarCodigoEnviado();
        iniciarSesion(respuesta);
    };

    const titulo = modo === 'login' ? 'Bienvenido de nuevo' : 'Crea tu cuenta';
    const subtitulo = modo === 'login' ? 'Inicia sesión para continuar.' : 'Únete a la comunidad de Forkly.';

    return (
        <AuthLayout titulo={titulo} subtitulo={subtitulo}>
            <div className='flex flex-col gap-8'>
                <AuthTabs modo={modo} setModo={setModo} />
                {/* Ambos formularios quedan montados para no perder lo escrito al cambiar de pestaña */}
                <div id='panel-login' role='tabpanel' aria-labelledby='tab-login' hidden={modo !== 'login'}>
                    <LoginForm onExito={iniciarSesion} />
                </div>
                <div id='panel-registro' role='tabpanel' aria-labelledby='tab-registro' hidden={modo !== 'registro'}>
                    <RegisterForm onExito={alRegistrarse} />
                </div>
            </div>
        </AuthLayout>
    );
};

export default AuthPage;
