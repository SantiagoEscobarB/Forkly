const Logo = () => {
    return (
        <div className='flex items-center gap-4'>
            <div className='flex h-12 w-12 items-center justify-center rounded-full bg-primary shadow-[0_0_24px] shadow-primary/50'>
                <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' strokeWidth='2'
                    strokeLinecap='round' strokeLinejoin='round' className='h-6 w-6 text-fondo'>
                    <path d='M6 3v7a2 2 0 0 0 2 2v9M10 3v7a2 2 0 0 1-2 2M8 3v5' />
                    <path d='M17 21V3c-2 1-3 4-3 7v3h3' />
                </svg>
            </div>
            <span className='font-display text-3xl font-bold tracking-tight'>FORKLY</span>
        </div>
    );
};

export default Logo;
