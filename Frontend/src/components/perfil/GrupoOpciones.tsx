import { CATEGORIAS, type CategoriaOpcion, type OpcionPerfil } from '../../types/perfil';
import { opcionesDe } from '../../utils/perfil';
import SelectorOpciones from './SelectorOpciones';

interface GrupoOpcionesProps {
    categoria: CategoriaOpcion;
    // Catálogo completo: el grupo muestra solo las opciones de su categoría
    opciones: OpcionPerfil[];
    seleccion: string[];
    onAlternar: (codigo: string) => void;
    deshabilitado?: boolean;
}

const GrupoOpciones = ({ categoria, opciones, seleccion, onAlternar, deshabilitado }: GrupoOpcionesProps) => {
    const { titulo, ayuda } = CATEGORIAS[categoria];
    const id = `grupo-${categoria.toLowerCase()}`;

    return (
        <section aria-labelledby={id} className='flex flex-col gap-3'>
            <div>
                <h3 id={id} className='font-display text-lg font-semibold'>{titulo}</h3>
                <p className='text-sm text-texto-suave'>{ayuda}</p>
            </div>
            <SelectorOpciones opciones={opcionesDe(opciones, categoria)} seleccion={seleccion}
                onAlternar={onAlternar} deshabilitado={deshabilitado} />
        </section>
    );
};

export default GrupoOpciones;
