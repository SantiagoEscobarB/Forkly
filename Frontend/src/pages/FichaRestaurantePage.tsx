import { useEffect, useRef, useState, type ReactNode } from 'react';
import { Link, useParams } from 'react-router';
import ErrorMessage from '../components/ui/ErrorMessage';
import Logo from '../components/ui/Logo';
import { RUTAS } from '../routes/rutas';
import { ApiError, mensajesDeError } from '../services/api';
import { obtenerFichaRestaurante } from '../services/restauranteService';
import type { FichaRestaurante } from '../types/restaurante';

// FOR-63, criterio 3: cada dato con su título en mayúsculas encima
const Dato = ({ titulo, children }: { titulo: string; children: ReactNode }) => (
    <div className='flex flex-col gap-2'>
        <dt className='text-sm tracking-widest text-texto-suave uppercase'>{titulo}</dt>
        <dd>{children}</dd>
    </div>
);

// Opciones del restaurante como etiquetas que no se pueden presionar
const Etiquetas = ({ nombres }: { nombres: string[] }) => (
    <ul className='flex flex-wrap gap-2'>
        {nombres.map((nombre) => (
            <li key={nombre} className='rounded-full border border-borde bg-fondo px-3 py-1 text-sm'>{nombre}</li>
        ))}
    </ul>
);

// Ficha de un restaurante (FOR-63). No guarda nada en el navegador ni en la base de datos.
const FichaRestaurantePage = () => {
    const { id = '' } = useParams();
    const [ficha, setFicha] = useState<FichaRestaurante | null>(null);
    const [noExiste, setNoExiste] = useState(false);
    const [errores, setErrores] = useState<string[]>([]);
    // Una consulta por dirección; en desarrollo, StrictMode ejecuta los efectos dos veces
    const consultado = useRef<string | null>(null);

    useEffect(() => {
        if (consultado.current === id) return;
        consultado.current = id;
        setFicha(null);
        setNoExiste(false);
        setErrores([]);
        obtenerFichaRestaurante(id)
            .then(setFicha)
            .catch((error: unknown) => {
                // Criterio 7: el restaurante no existe (p. ej. /restaurantes/999 o /restaurantes/abc)
                if (error instanceof ApiError && error.status === 404) setNoExiste(true);
                else setErrores(mensajesDeError(error));
            });
    }, [id]);

    if (noExiste)
        return (
            <main className='mx-auto flex min-h-screen w-full max-w-3xl flex-col items-start gap-8 px-4 py-12'>
                <Logo />
                <p className='text-texto-suave'>No encontramos este restaurante.</p>
                <Link to={RUTAS.restaurantes}
                    className='rounded-full bg-primary px-6 py-3 font-semibold text-fondo transition-colors hover:bg-primary-hover'>
                    Ver todos los restaurantes
                </Link>
            </main>
        );

    return (
        <main className='mx-auto flex min-h-screen w-full max-w-3xl flex-col gap-8 px-4 py-12'>
            <div className='flex items-center justify-between gap-4'>
                <Logo />
                <Link to={RUTAS.restaurantes} className='text-sm text-texto-suave hover:text-white'>
                    Volver a restaurantes
                </Link>
            </div>

            {errores.length > 0 ? (
                <ErrorMessage errores={errores} />
            ) : !ficha ? (
                <p className='text-texto-suave'>Cargando restaurante...</p>
            ) : (
                <>
                    <div>
                        <h1 className='font-display text-3xl font-bold'>{ficha.nombre}</h1>
                        <p className='mt-1 text-texto-suave'>{`${ficha.rangoPrecio} por persona`}</p>
                    </div>

                    <dl className='flex flex-col gap-6 rounded-3xl border border-borde bg-panel/40 p-6'>
                        <Dato titulo='Dirección'>{`${ficha.direccion} · ${ficha.barrio}`}</Dato>
                        <Dato titulo='Teléfono'>
                            {/* Criterio 4: en un celular abre la aplicación de llamadas con ese número */}
                            <a href={`tel:${ficha.telefono.replace(/ /g, '')}`} className='text-primary hover:underline'>
                                {ficha.telefono}
                            </a>
                        </Dato>
                        <Dato titulo='Tipos de cocina'><Etiquetas nombres={ficha.tiposCocina} /></Dato>
                        <Dato titulo='Ambientes'><Etiquetas nombres={ficha.ambientes} /></Dato>
                    </dl>

                    {/* Criterios 5 y 9: la dirección completa se parte en varias líneas si no cabe */}
                    <p className='text-sm text-texto-suave'>
                        Datos tomados de{' '}
                        <a href={ficha.fuenteUrl} target='_blank' rel='noopener noreferrer'
                            className='break-all text-primary hover:underline'>
                            {ficha.fuenteUrl}
                        </a>
                    </p>
                </>
            )}
        </main>
    );
};

export default FichaRestaurantePage;
