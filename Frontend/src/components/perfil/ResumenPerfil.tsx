import { CATEGORIAS, ORDEN_CATEGORIAS, type Perfil } from '../../types/perfil';
import { opcionesDe } from '../../utils/perfil';

interface ResumenPerfilProps {
    perfil: Perfil | null;
}

// Preferencias del usuario en el inicio: así se ve que se restauran al iniciar sesión (H01.04)
const ResumenPerfil = ({ perfil }: ResumenPerfilProps) => {
    if (!perfil) return <p className='text-texto-suave'>Cargando tus preferencias...</p>;

    if (perfil.seleccionadas.length === 0)
        return (
            <p className='text-texto-suave'>
                Todavía no configuras tu perfil gastronómico. Puedes hacerlo cuando quieras.
            </p>
        );

    return (
        <section aria-label='Tu perfil gastronómico'
            className='flex w-full max-w-xl flex-col gap-4 rounded-3xl border border-borde bg-panel/60 p-6 text-left'>
            <h2 className='font-display text-xl font-semibold'>Tus preferencias</h2>
            {ORDEN_CATEGORIAS.map((categoria) => {
                const elegidas = opcionesDe(perfil.seleccionadas, categoria);
                if (elegidas.length === 0) return null;
                return (
                    <div key={categoria} className='flex flex-col gap-2'>
                        <p className='text-sm tracking-widest text-texto-suave uppercase'>{CATEGORIAS[categoria].titulo}</p>
                        <ul className='flex flex-wrap gap-2'>
                            {elegidas.map((opcion) => (
                                <li key={opcion.codigo}
                                    className='rounded-full border border-borde bg-fondo px-3 py-1 text-sm'>
                                    {opcion.nombre}
                                </li>
                            ))}
                        </ul>
                    </div>
                );
            })}
        </section>
    );
};

export default ResumenPerfil;
