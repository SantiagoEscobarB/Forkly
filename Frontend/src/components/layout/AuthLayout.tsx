import type { ReactNode } from 'react';
import AuthHero from '../auth/AuthHero';
import Logo from '../ui/Logo';

interface AuthLayoutProps {
    titulo: string;
    subtitulo: ReactNode;
    children: ReactNode;
}

// Diseño de dos columnas del Figma: hero a la izquierda y contenido a la derecha
const AuthLayout = ({ titulo, subtitulo, children }: AuthLayoutProps) => {
    return (
        <main className='grid min-h-screen lg:grid-cols-2'>
            <AuthHero />
            <section className='flex items-center justify-center px-4 py-12 sm:px-8'>
                <div className='flex w-full max-w-md flex-col gap-8'>
                    <div className='lg:hidden'><Logo /></div>
                    <div>
                        <h2 className='font-display text-3xl font-bold'>{titulo}</h2>
                        <p className='mt-1 text-texto-suave'>{subtitulo}</p>
                    </div>
                    {children}
                </div>
            </section>
        </main>
    );
};

export default AuthLayout;
