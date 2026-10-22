import { useState, type InputHTMLAttributes } from 'react';

interface InputFieldProps extends InputHTMLAttributes<HTMLInputElement> {
    label: string;
}

const InputField = ({ label, type = 'text', ...props }: InputFieldProps) => {
    const [visible, setVisible] = useState(false);
    const esContrasena = type === 'password';

    return (
        <label className='flex flex-col gap-2'>
            <span className='text-sm tracking-widest text-texto-suave uppercase'>{label}</span>
            <div className='relative'>
                <input {...props} type={esContrasena && visible ? 'text' : type}
                    className={`w-full rounded-full border border-borde bg-panel py-4 pl-5 text-white
                               placeholder:text-texto-suave/80 outline-none transition-colors
                               focus:border-primary scheme-dark ${esContrasena ? 'pr-12' : 'pr-5'}`} />
                {esContrasena && (
                    <button type='button' onClick={() => setVisible(!visible)}
                        aria-label='Mostrar contraseña' aria-pressed={visible}
                        className='absolute right-5 top-1/2 -translate-y-1/2 text-texto-suave hover:text-white'>
                        <svg viewBox='0 0 24 24' fill='none' stroke='currentColor' strokeWidth='2'
                            aria-hidden='true' className='h-5 w-5'>
                            <path d='M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12Z' />
                            <circle cx='12' cy='12' r='3' />
                            {visible && <path d='M3 3l18 18' />}
                        </svg>
                    </button>
                )}
            </div>
        </label>
    );
};

export default InputField;
