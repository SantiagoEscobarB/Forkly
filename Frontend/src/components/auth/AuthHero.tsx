import Logo from '../ui/Logo';

// Placeholder: luego se traerán los valores reales del backend
const estadisticas = [
    { valor: '0', etiqueta: 'Restaurantes' },
    { valor: '0', etiqueta: 'Usuarios' },
    { valor: '0%', etiqueta: 'Satisfacción' },
];

// Círculos difuminados que imitan el bokeh de la foto del diseño.
// Cuando tengamos la imagen, se agrega en assets/ y se pone de fondo aquí.
const luces = [
    'left-[55%] top-[30%] h-40 w-40',
    'left-[75%] top-[45%] h-28 w-28',
    'left-[10%] top-[60%] h-24 w-24',
    'left-[35%] top-[70%] h-16 w-16',
    'left-[85%] top-[15%] h-20 w-20',
];

const anio = new Date().getFullYear();

const AuthHero = () => {
    return (
        <section className='relative hidden overflow-hidden bg-[#1E140D] lg:flex lg:flex-col lg:justify-center lg:px-14'>
            {luces.map((clases) => (
                <div key={clases} className={`absolute rounded-full bg-primary/25 blur-2xl ${clases}`} />
            ))}
            <div className='absolute inset-0 bg-linear-to-r from-black/60 via-black/30 to-transparent' />

            <div className='relative flex max-w-lg flex-col gap-6'>
                <Logo />
                <h1 className='font-display text-5xl font-bold leading-tight'>
                    Descubre el sabor para cada momento
                </h1>
                <p className='text-xl leading-relaxed text-white/70'>
                    Recomendaciones personalizadas con IA, diario gastronómico y una comunidad
                    de personas con tus mismos gustos.
                </p>
                <div className='flex gap-4'>
                    {estadisticas.map(({ valor, etiqueta }) => (
                        <div key={etiqueta}
                            className='rounded-2xl border border-white/15 bg-white/10 px-5 py-3 text-center backdrop-blur-md'>
                            <p className='font-display text-2xl font-bold'>{valor}</p>
                            <p className='text-sm text-white/70'>{etiqueta}</p>
                        </div>
                    ))}
                </div>
                <p className='text-sm text-white/40'>© {anio} Forkly · Medellín, Colombia</p>
            </div>
        </section>
    );
};

export default AuthHero;
