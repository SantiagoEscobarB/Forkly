import { useState, type FormEvent } from 'react';
import { mensajesDeError } from '../../services/api';
import { registro } from '../../services/authService';
import type { AuthResponse, RegistroRequest } from '../../types/auth';
import {
    cumpleReglasContrasena, FECHA_MINIMA, fechaLocalHoy, MENSAJE_USERNAME, PATRON_USERNAME,
} from '../../utils/validaciones';
import ErrorMessage from '../ui/ErrorMessage';
import InputField from '../ui/InputField';
import ReglasContrasena from '../ui/ReglasContrasena';
import SubmitButton from '../ui/SubmitButton';

const datosIniciales: RegistroRequest = {
    nombreCompleto: '',
    userName: '',
    email: '',
    password: '',
    fechaNacimiento: '',
};

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

    const contrasenaValida = cumpleReglasContrasena(datos.password);

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
            <InputField label='Nombre de usuario' required pattern={PATRON_USERNAME}
                title={MENSAJE_USERNAME} placeholder='juanatehortua'
                autoComplete='username'
                value={datos.userName} onChange={(e) => actualizar('userName', e.target.value)} />
            <InputField label='Correo electrónico' type='email' required placeholder='juan@ejemplo.com'
                autoComplete='email'
                value={datos.email} onChange={(e) => actualizar('email', e.target.value)} />
            {/* "hoy" se refresca al enfocar por si la pestaña quedó abierta pasada la medianoche */}
            <InputField label='Fecha de nacimiento' type='date' required min={FECHA_MINIMA} max={hoy}
                onFocus={() => setHoy(fechaLocalHoy())}
                value={datos.fechaNacimiento} onChange={(e) => actualizar('fechaNacimiento', e.target.value)} />
            <div className='flex flex-col gap-3'>
                <InputField label='Contraseña' type='password' required placeholder='••••••••'
                    autoComplete='new-password' aria-describedby='reglas-contrasena'
                    value={datos.password} onChange={(e) => actualizar('password', e.target.value)} />
                <ReglasContrasena id='reglas-contrasena' contrasena={datos.password} />
            </div>
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando || !contrasenaValida}>
                {cargando ? 'Creando cuenta...' : 'Crear cuenta'}
            </SubmitButton>
        </form>
    );
};

export default RegisterForm;
