import type { OpcionPerfil } from '../../types/perfil';

interface SelectorOpcionesProps {
    opciones: OpcionPerfil[];
    seleccion: string[];
    onAlternar: (codigo: string) => void;
    deshabilitado?: boolean;
}

// Botones tipo píldora que se encienden al elegirlos (aria-pressed para lectores de pantalla)
const SelectorOpciones = ({ opciones, seleccion, onAlternar, deshabilitado = false }: SelectorOpcionesProps) => {
    return (
        <div className='flex flex-wrap gap-2'>
            {opciones.map((opcion) => {
                const elegida = seleccion.includes(opcion.codigo);
                return (
                    <button key={opcion.codigo} type='button' aria-pressed={elegida} disabled={deshabilitado}
                        onClick={() => onAlternar(opcion.codigo)}
                        className={`rounded-full border px-4 py-2 text-sm font-semibold transition-colors disabled:opacity-60
                                    ${elegida
                                        ? 'border-primary bg-primary text-fondo'
                                        : 'border-borde bg-panel text-white hover:border-primary'}`}>
                        {opcion.nombre}
                        {/* El espacio va como texto para que el nombre accesible sea "$$ De $25.000 a $50.000" */}
                        {opcion.detalle && ' '}
                        {opcion.detalle && (
                            <span className={`ml-1 font-normal ${elegida ? 'text-fondo/80' : 'text-texto-suave'}`}>
                                {opcion.detalle}
                            </span>
                        )}
                    </button>
                );
            })}
        </div>
    );
};

export default SelectorOpciones;
