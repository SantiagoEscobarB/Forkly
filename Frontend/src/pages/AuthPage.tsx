import { useState } from 'react';
import AuthHero from '../components/AuthHero';
import AuthTabs, { type Modo } from '../components/AuthTabs';
import LoginForm from '../components/LoginForm';
import Logo from '../components/Logo';
import RegisterForm from '../components/RegisterForm';
import SocialButtons from '../components/SocialButtons';
import type { AuthResponse } from '../types/auth';

const AuthPage = () => {
    const [modo, setModo] = useState<Modo>('login');
    const [sesion, setSesion] = useState<AuthResponse | null>(null);

    return (
        <main className='grid min-h-screen lg:grid-cols-2'>
            <AuthHero />
            <section className='flex items-center justify-center px-4 py-12 sm:px-8'>
                <div className='flex w-full max-w-md flex-col gap-8'>
                    <div className='lg:hidden'><Logo /></div>
                    <div>
                        <h2 className='font-display text-3xl font-bold'>
                            {modo === 'login' ? 'Bienvenido de nuevo' : 'Crea tu cuenta'}
                        </h2>
                        <p className='mt-1 text-texto-suave'>
                            {modo === 'login' ? 'Inicia sesión para continuar.' : 'Únete a la comunidad de Forkly.'}
                        </p>
                    </div>
                    {/* Temporal hasta tener router: confirma que el login o registro funcionó */}
                    {sesion && (
                        <p role='status' className='rounded-2xl border border-green-400/40 bg-green-400/10 px-5 py-3 text-green-300'>
                            ¡Hola, {sesion.usuario.nombreCompleto}! Iniciaste sesión correctamente.
                        </p>
                    )}
                    <AuthTabs modo={modo} setModo={setModo} />
                    {/* Ambos formularios quedan montados para no perder lo escrito al cambiar de pestaña */}
                    <div id='panel-login' role='tabpanel' aria-labelledby='tab-login' hidden={modo !== 'login'}>
                        <LoginForm onExito={setSesion} />
                    </div>
                    <div id='panel-registro' role='tabpanel' aria-labelledby='tab-registro' hidden={modo !== 'registro'}>
                        <RegisterForm onExito={setSesion} />
                    </div>
                    <SocialButtons />
                </div>
            </section>
        </main>
    );
};

export default AuthPage;
