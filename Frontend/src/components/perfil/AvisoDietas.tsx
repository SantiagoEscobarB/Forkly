// H01.10, criterio 2: advierte que hay más de un tipo de dieta elegido
const AvisoDietas = ({ visible }: { visible: boolean }) => {
    if (!visible) return null;

    return (
        <p role='alert' className='rounded-2xl border border-red-400/40 bg-red-400/10 px-4 py-3 text-sm text-red-300'>
            Elegiste más de un tipo de dieta. Deja solo una (vegetariana, vegana o pescetariana) para poder guardar.
        </p>
    );
};

export default AvisoDietas;
