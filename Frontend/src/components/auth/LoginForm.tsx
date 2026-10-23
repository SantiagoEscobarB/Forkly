import { useState, type FormEvent } from 'react';
import { mensajesDeError } from '../../services/api';
import { login } from '../../services/authService';
import type { AuthResponse, LoginRequest } from '../../types/auth';
import ErrorMessage from '../ui/ErrorMessage';
import InputField from '../ui/InputField';
import SubmitButton from '../ui/SubmitButton';

interface LoginFormProps {
    onExito: (respuesta: AuthResponse) => void;
}

const LoginForm = ({ onExito }: LoginFormProps) => {
    const [datos, setDatos] = useState<LoginRequest>({ userName: '', password: '' });
    const [errores, setErrores] = useState<string[]>([]);
    const [cargando, setCargando] = useState(false);

    const enviar = async (e: FormEvent) => {
        e.preventDefault();
        setErrores([]);
        setCargando(true);
        try {
            onExito(await login({ ...datos, userName: datos.userName.trim() }));
        } catch (error) {
            setErrores(mensajesDeError(error, { 401: 'Usuario o contraseña incorrectos.' }));
        } finally {
            setCargando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            <InputField label='Nombre de usuario' required placeholder='juanatehortua' autoComplete='username'
                value={datos.userName} onChange={(e) => setDatos((prev) => ({ ...prev, userName: e.target.value }))} />
            <div className='flex flex-col gap-3'>
                <InputField label='Contraseña' type='password' required placeholder='••••••••' autoComplete='current-password'
                    value={datos.password} onChange={(e) => setDatos((prev) => ({ ...prev, password: e.target.value }))} />
                <button type='button' className='self-end text-sm font-semibold text-primary hover:underline'>
                    ¿Olvidaste tu contraseña?
                </button>
            </div>
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando}>{cargando ? 'Entrando...' : 'Entrar'}</SubmitButton>
        </form>
    );
};

export default LoginForm;
