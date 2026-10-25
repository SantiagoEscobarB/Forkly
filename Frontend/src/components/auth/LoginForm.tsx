import { useState, type FormEvent } from 'react';
import { Link } from 'react-router';
import { RUTAS } from '../../routes/rutas';
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
            // 401: mensaje genérico que no dice qué dato falló. 429: el backend explica el bloqueo temporal.
            setErrores(mensajesDeError(error, { 401: 'Usuario o contraseña incorrectos.' }));
        } finally {
            setCargando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            {/* El backend acepta el nombre de usuario o el correo */}
            <InputField label='Usuario o correo' required placeholder='juanatehortua' autoComplete='username'
                value={datos.userName} onChange={(e) => setDatos((prev) => ({ ...prev, userName: e.target.value }))} />
            <div className='flex flex-col gap-3'>
                <InputField label='Contraseña' type='password' required placeholder='••••••••' autoComplete='current-password'
                    value={datos.password} onChange={(e) => setDatos((prev) => ({ ...prev, password: e.target.value }))} />
                <Link to={RUTAS.recuperarContrasena} className='self-end text-sm font-semibold text-primary hover:underline'>
                    ¿Olvidaste tu contraseña?
                </Link>
            </div>
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando}>{cargando ? 'Entrando...' : 'Entrar'}</SubmitButton>
        </form>
    );
};

export default LoginForm;
