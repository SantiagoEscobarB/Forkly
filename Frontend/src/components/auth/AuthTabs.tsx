export type Modo = 'login' | 'registro';

interface AuthTabsProps {
    modo: Modo;
    setModo: (modo: Modo) => void;
}

const pestanas: { modo: Modo; texto: string }[] = [
    { modo: 'login', texto: 'Iniciar sesión' },
    { modo: 'registro', texto: 'Registrarse' },
];

const AuthTabs = ({ modo, setModo }: AuthTabsProps) => {
    return (
        <div role='tablist' className='grid grid-cols-2 gap-1 rounded-full bg-panel/70 p-1'>
            {pestanas.map((pestana) => (
                <button key={pestana.modo} type='button' role='tab'
                    id={`tab-${pestana.modo}`} aria-controls={`panel-${pestana.modo}`}
                    aria-selected={modo === pestana.modo} onClick={() => setModo(pestana.modo)}
                    className={`rounded-full py-3 transition-colors ${modo === pestana.modo
                        ? 'bg-primary text-fondo'
                        : 'text-texto-suave hover:text-white'}`}>
                    {pestana.texto}
                </button>
            ))}
        </div>
    );
};

export default AuthTabs;
