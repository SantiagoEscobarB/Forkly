interface ProgresoPasosProps {
    paso: number;
    total: number;
}

const ProgresoPasos = ({ paso, total }: ProgresoPasosProps) => {
    return (
        <div role='progressbar' aria-label={`Paso ${paso} de ${total}`}
            aria-valuemin={1} aria-valuemax={total} aria-valuenow={paso} className='flex gap-2'>
            {Array.from({ length: total }, (_, indice) => (
                <div key={indice} className={`h-1.5 flex-1 rounded-full ${indice < paso ? 'bg-primary' : 'bg-panel'}`} />
            ))}
        </div>
    );
};

export default ProgresoPasos;
