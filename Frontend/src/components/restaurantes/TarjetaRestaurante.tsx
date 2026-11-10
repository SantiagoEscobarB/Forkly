import { Link } from 'react-router';
import { rutaFichaRestaurante } from '../../routes/rutas';
import type { RestauranteResumen } from '../../types/restaurante';

interface TarjetaRestauranteProps {
    restaurante: RestauranteResumen;
}

// FOR-62, criterio 4: tres líneas (nombre; tipos de cocina; barrio y rango de precio).
// Al presionarla abre la ficha del restaurante (FOR-63).
const TarjetaRestaurante = ({ restaurante }: TarjetaRestauranteProps) => {
    return (
        <Link to={rutaFichaRestaurante(restaurante.id)}
            className='flex h-full flex-col gap-1 rounded-3xl border border-borde bg-panel/40 p-5 transition-colors hover:border-primary'>
            <span className='font-display text-lg font-semibold'>{restaurante.nombre}</span>
            <span className='text-sm text-texto-suave'>{restaurante.tiposCocina.join(' · ')}</span>
            <span className='text-sm text-texto-suave'>{`${restaurante.barrio} · ${restaurante.rangoPrecio}`}</span>
        </Link>
    );
};

export default TarjetaRestaurante;
