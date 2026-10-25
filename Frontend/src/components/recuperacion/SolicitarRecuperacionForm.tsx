import { useState, type FormEvent } from 'react';
import { mensajesDeError } from '../../services/api';
import { solicitarRecuperacion } from '../../services/recuperacionService';
import ErrorMessage from '../ui/ErrorMessage';
import InputField from '../ui/InputField';
import SubmitButton from '../ui/SubmitButton';

interface SolicitarRecuperacionFormProps {
    emailInicial: string;
    onEnviado: (email: string) => void;
}

const SolicitarRecuperacionForm = ({ emailInicial, onEnviado }: SolicitarRecuperacionFormProps) => {
    const [email, setEmail] = useState(emailInicial);
    const [errores, setErrores] = useState<string[]>([]);
    const [cargando, setCargando] = useState(false);

    const enviar = async (e: FormEvent) => {
        e.preventDefault();
        setErrores([]);
        setCargando(true);
        const correo = email.trim();
        try {
            await solicitarRecuperacion({ email: correo });
            onEnviado(correo);
        } catch (error) {
            // 429: el backend limita las solicitudes y explica cuánto esperar
            setErrores(mensajesDeError(error));
        } finally {
            setCargando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            <InputField label='Correo electrónico' type='email' required placeholder='juan@ejemplo.com'
                autoComplete='email' value={email} onChange={(e) => setEmail(e.target.value)} />
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando}>{cargando ? 'Enviando...' : 'Enviar enlace'}</SubmitButton>
        </form>
    );
};

export default SolicitarRecuperacionForm;
