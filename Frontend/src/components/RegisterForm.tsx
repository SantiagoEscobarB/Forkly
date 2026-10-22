import { useState, type FormEvent } from 'react';
import { mensajesDeError } from '../services/api';
import { registro } from '../services/authService';
import type { AuthResponse, RegistroRequest } from '../types/auth';
import ErrorMessage from './ErrorMessage';
import InputField from './InputField';
import SubmitButton from './SubmitButton';

const datosIniciales: RegistroRequest = {
    nombreCompleto: '',
    userName: '',
    email: '',
    password: '',
    fechaNacimiento: '',
};

// Fecha local en "yyyy-MM-dd" (toISOString usa UTC y en Colombia daría mañana desde las 7 p. m.)
const fechaLocalHoy = () => {
    const d = new Date();
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
};

// Mismas reglas que el PasswordValidator por defecto de Identity
// (solo cuenta letras y números ASCII, igual que el backend).
const reglasContrasena = [
    { texto: 'Mínimo 6 caracteres', cumple: (p: string) => p.length >= 6 },
    { texto: 'Una mayúscula', cumple: (p: string) => /[A-Z]/.test(p) },
    { texto: 'Una minúscula', cumple: (p: string) => /[a-z]/.test(p) },
    { texto: 'Un número', cumple: (p: string) => /[0-9]/.test(p) },
    { texto: 'Un símbolo', cumple: (p: string) => /[^a-zA-Z0-9]/.test(p) },
];

interface RegisterFormProps {
    onExito: (respuesta: AuthResponse) => void;
}

const RegisterForm = ({ onExito }: RegisterFormProps) => {
    const [datos, setDatos] = useState<RegistroRequest>(datosIniciales);
    const [hoy, setHoy] = useState(fechaLocalHoy);
    const [errores, setErrores] = useState<string[]>([]);
    const [cargando, setCargando] = useState(false);

    const actualizar = (campo: keyof RegistroRequest, valor: string) =>
        setDatos((prev) => ({ ...prev, [campo]: valor }));

    const contrasenaValida = reglasContrasena.every((regla) => regla.cumple(datos.password));

    const enviar = async (e: FormEvent) => {
        e.preventDefault();
        if (!contrasenaValida) return;
        setErrores([]);
        setCargando(true);
        try {
            onExito(await registro({
                ...datos,
                nombreCompleto: datos.nombreCompleto.trim(),
                userName: datos.userName.trim(),
                email: datos.email.trim(),
            }));
        } catch (error) {
            setErrores(mensajesDeError(error));
        } finally {
            setCargando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            <InputField label='Nombre completo' required maxLength={100} pattern='.*\S.*'
                title='El nombre no puede estar vacío' placeholder='Juan Atehortua' autoComplete='name'
                value={datos.nombreCompleto} onChange={(e) => actualizar('nombreCompleto', e.target.value)} />
            <InputField label='Nombre de usuario' required pattern='[a-zA-Z0-9._\-]+'
                title='Solo letras, números, puntos, guiones y guion bajo' placeholder='juanatehortua'
                autoComplete='username'
                value={datos.userName} onChange={(e) => actualizar('userName', e.target.value)} />
            <InputField label='Correo electrónico' type='email' required placeholder='juan@ejemplo.com'
                autoComplete='email'
                value={datos.email} onChange={(e) => actualizar('email', e.target.value)} />
            {/* "hoy" se refresca al enfocar por si la pestaña quedó abierta pasada la medianoche */}
            <InputField label='Fecha de nacimiento' type='date' required min='1900-01-01' max={hoy}
                onFocus={() => setHoy(fechaLocalHoy())}
                value={datos.fechaNacimiento} onChange={(e) => actualizar('fechaNacimiento', e.target.value)} />
            <div className='flex flex-col gap-3'>
                <InputField label='Contraseña' type='password' required placeholder='••••••••'
                    autoComplete='new-password' aria-describedby='reglas-contrasena'
                    value={datos.password} onChange={(e) => actualizar('password', e.target.value)} />
                <ul id='reglas-contrasena' className='grid grid-cols-2 gap-x-4 gap-y-1 px-2 text-sm'>
                    {reglasContrasena.map((regla) => {
                        const cumple = regla.cumple(datos.password);
                        return (
                            <li key={regla.texto} className={cumple ? 'text-green-400' : 'text-texto-suave'}>
                                <span aria-hidden='true'>{cumple ? '✓' : '•'}</span> {regla.texto}
                                {cumple && <span className='sr-only'> (cumplido)</span>}
                            </li>
                        );
                    })}
                </ul>
            </div>
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando || !contrasenaValida}>
                {cargando ? 'Creando cuenta...' : 'Crear cuenta'}
            </SubmitButton>
        </form>
    );
};

export default RegisterForm;
