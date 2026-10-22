import type { ButtonHTMLAttributes } from 'react';

const SubmitButton = ({ className = '', ...props }: ButtonHTMLAttributes<HTMLButtonElement>) => {
    return (
        <button type='submit' {...props}
            className={`w-full rounded-full bg-primary py-4 text-lg font-semibold text-fondo transition-colors
                        hover:bg-primary-hover disabled:opacity-60 ${className}`} />
    );
};

export default SubmitButton;
