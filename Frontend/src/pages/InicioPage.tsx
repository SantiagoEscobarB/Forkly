import { useEffect, useState } from 'react';
import { Link } from 'react-router';
import ResumenPerfil from '../components/perfil/ResumenPerfil';
import Logo from '../components/ui/Logo';
import { useSesion } from '../hooks/useSesion';
import { RUTAS } from '../routes/rutas';
import { obtenerPerfil } from '../services/perfilService';
import type { Perfil } from '../types/perfil';

// Placeholder hasta diseñar el inicio de la app. Muestra las preferencias guardadas,
// que se restauran al iniciar sesión (H01.04), y lleva a los ajustes del perfil (H01.09).
const InicioPage = () => {
    const { sesion, cerrarSesion } = useSesion();
    const [perfil, setPerfil] = useState<Perfil | null>(null);

    useEffect(() => {
        let cancelado = false;
        obtenerPerfil()
            .then((respuesta) => {
                if (!cancelado) setPerfil(respuesta);
            })
            .catch(() => { /* sin perfil se muestra el saludo igual */ });
        return () => {
            cancelado = true;
        };
    }, []);

    const sinPreferencias = !perfil || perfil.seleccionadas.length === 0;

    return (
        <main className='flex min-h-screen flex-col items-center justify-center gap-6 px-4 py-12 text-center'>
            <Logo />
            <h1 className='font-display text-3xl font-bold'>¡Hola, {sesion?.usuario.nombreCompleto}!</h1>
            <p className='text-texto-suave'>Tu cuenta está verificada.</p>
            <ResumenPerfil perfil={perfil} />
            <div className='flex flex-wrap justify-center gap-3'>
                <Link to={RUTAS.perfilGastronomico}
                    className='rounded-full bg-primary px-6 py-3 font-semibold text-fondo transition-colors hover:bg-primary-hover'>
                    {sinPreferencias ? 'Configurar mi perfil gastronómico' : 'Editar mi perfil gastronómico'}
                </Link>
                <button type='button' onClick={cerrarSesion}
                    className='rounded-full border border-borde bg-panel px-6 py-3 font-semibold transition-colors hover:border-primary'>
                    Cerrar sesión
                </button>
            </div>
        </main>
    );
};

export default InicioPage;
