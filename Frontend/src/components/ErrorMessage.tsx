interface ErrorMessageProps {
    errores: string[];
}

const ErrorMessage = ({ errores }: ErrorMessageProps) => {
    if (errores.length === 0) return null;

    return (
        <ul role='alert' className='flex flex-col gap-1 text-sm text-red-400'>
            {errores.map((error) => <li key={error}>{error}</li>)}
        </ul>
    );
};

export default ErrorMessage;
