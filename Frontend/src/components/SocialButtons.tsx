// Pendiente: el backend todavía no tiene login con Google.
const SocialButtons = () => {
    return (
        <>
            <div className='flex items-center gap-4 text-sm text-texto-suave'>
                <div className='h-px flex-1 bg-borde' />
                o continúa con
                <div className='h-px flex-1 bg-borde' />
            </div>
            <button type='button'
                className='rounded-full border border-borde bg-panel py-3 font-semibold transition-colors hover:border-primary'>
                Google
            </button>
        </>
    );
};

export default SocialButtons;
