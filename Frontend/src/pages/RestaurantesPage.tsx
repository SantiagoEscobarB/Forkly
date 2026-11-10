import { useEffect, useRef, useState } from 'react';
import { Link } from 'react-router';
import SelectorOpciones from '../components/perfil/SelectorOpciones';
import TarjetaRestaurante from '../components/restaurantes/TarjetaRestaurante';
import ErrorMessage from '../components/ui/ErrorMessage';
import InputField from '../components/ui/InputField';
import Logo from '../components/ui/Logo';
import { RUTAS } from '../routes/rutas';
import { mensajesDeError } from '../services/api';
import { obtenerOpciones } from '../services/perfilService';
import { obtenerRestaurantes } from '../services/restauranteService';
import type { OpcionPerfil } from '../types/perfil';
import type { RestauranteResumen } from '../types/restaurante';
import { alternar, opcionesDe } from '../utils/perfil';
import { filtrarRestaurantes, ordenarPorNombre, textoContador } from '../utils/restaurantes';

// FOR-62, criterio 6
const MAXIMO_BUSQUEDA = 100;

// Búsqueda de restaurantes por nombre o por tipo de cocina (FOR-62).
// El texto y los tipos elegidos solo viven en el estado de la página (criterio 10).
const RestaurantesPage = () => {
    const [cocinas, setCocinas] = useState<OpcionPerfil[]>([]);
    const [restaurantes, setRestaurantes] = useState<RestauranteResumen[]>([]);
    const [cargando, setCargando] = useState(true);
    const [errores, setErrores] = useState<string[]>([]);
    const [texto, setTexto] = useState('');
    const [seleccion, setSeleccion] = useState<string[]>([]); // códigos de los tipos de cocina elegidos
    // Criterio 9: una sola consulta al abrir la página. En desarrollo, StrictMode ejecuta los
    // efectos dos veces; la referencia evita la segunda consulta.
    const consultado = useRef(false);

    useEffect(() => {
        if (consultado.current) return;
        consultado.current = true;
        Promise.all([obtenerOpciones(), obtenerRestaurantes()])
            .then(([catalogo, lista]) => {
                setCocinas(opcionesDe(catalogo, 'COCINA'));
                setRestaurantes(ordenarPorNombre(lista));
                setCargando(false);
            })
            .catch((error: unknown) => {
                setErrores(mensajesDeError(error));
                setCargando(false);
            });
    }, []);

    const limpiarBusqueda = () => {
        setTexto('');
        setSeleccion([]);
    };

    const nombresElegidos = cocinas
        .filter((cocina) => seleccion.includes(cocina.codigo))
        .map((cocina) => cocina.nombre);
    const visibles = filtrarRestaurantes(restaurantes, texto, nombresElegidos);
    const falloCarga = errores.length > 0;

    return (
        <main className='mx-auto flex min-h-screen w-full max-w-5xl flex-col gap-8 px-4 py-12'>
            <div className='flex items-center justify-between gap-4'>
                <Logo />
                <Link to={RUTAS.inicio} className='text-sm text-texto-suave hover:text-white'>Volver al inicio</Link>
            </div>
            <div>
                <h1 className='font-display text-3xl font-bold'>Restaurantes</h1>
                {/* Criterio 11: con un error solo quedan el logo, el enlace, el título y el mensaje */}
                {!falloCarga && <p className='mt-1 text-texto-suave'>Busca por nombre o por tipo de cocina.</p>}
            </div>

            {falloCarga ? (
                <ErrorMessage errores={errores} />
            ) : cargando ? (
                <p className='text-texto-suave'>Cargando restaurantes...</p>
            ) : (
                <>
                    <InputField label='Buscar' placeholder='Nombre del restaurante' maxLength={MAXIMO_BUSQUEDA}
                        autoComplete='off' value={texto} onChange={(e) => setTexto(e.target.value)} />

                    <section aria-labelledby='titulo-tipos-cocina' className='flex flex-col gap-3'>
                        <h2 id='titulo-tipos-cocina' className='font-display text-lg font-semibold'>Tipos de cocina</h2>
                        <SelectorOpciones opciones={cocinas} seleccion={seleccion}
                            onAlternar={(codigo) => setSeleccion((actual) => alternar(actual, codigo))} />
                    </section>

                    {visibles.length === 0 ? (
                        <div className='flex flex-col items-start gap-4'>
                            <p className='text-texto-suave'>No encontramos restaurantes con esa búsqueda.</p>
                            <button type='button' onClick={limpiarBusqueda}
                                className='rounded-full border border-borde bg-panel px-6 py-3 font-semibold transition-colors hover:border-primary'>
                                Limpiar búsqueda
                            </button>
                        </div>
                    ) : (
                        <div className='flex flex-col gap-4'>
                            <p className='text-sm text-texto-suave'>{textoContador(visibles.length)}</p>
                            <ul className='grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3'>
                                {visibles.map((restaurante) => (
                                    <li key={restaurante.id}>
                                        <TarjetaRestaurante restaurante={restaurante} />
                                    </li>
                                ))}
                            </ul>
                        </div>
                    )}
                </>
            )}
        </main>
    );
};

export default RestaurantesPage;
