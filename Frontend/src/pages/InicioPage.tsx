import Logo from '../components/ui/Logo';
import { useSesion } from '../hooks/useSesion';

// Placeholder hasta diseñar el inicio de la app
const InicioPage = () => {
    const { sesion, cerrarSesion } = useSesion();

    return (
        <main className='flex min-h-screen flex-col items-center justify-center gap-6 px-4 text-center'>
            <Logo />
            <h1 className='font-display text-3xl font-bold'>¡Hola, {sesion?.usuario.nombreCompleto}!</h1>
            <p className='text-texto-suave'>Tu cuenta está verificada.</p>
            <button type='button' onClick={cerrarSesion}
                className='rounded-full border border-borde bg-panel px-6 py-3 font-semibold transition-colors hover:border-primary'>
                Cerrar sesión
            </button>
        </main>
    );
};

export default InicioPage;
