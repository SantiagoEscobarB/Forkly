import { useState, type FormEvent } from 'react';
import { mensajesDeError } from '../../services/api';
import { registroGoogle } from '../../services/authService';
import type { AuthResponse, GoogleRegistroRequest } from '../../types/auth';
import { FECHA_MINIMA, fechaLocalHoy, MENSAJE_USERNAME, PATRON_USERNAME } from '../../utils/validaciones';
import ErrorMessage from '../ui/ErrorMessage';
import InputField from '../ui/InputField';
import SubmitButton from '../ui/SubmitButton';

export interface GooglePendiente {
    idToken: string;
    nombreCompleto: string;
    email: string;
}

interface GoogleRegistroFormProps {
    pendiente: GooglePendiente;
    onExito: (respuesta: AuthResponse) => void;
    onCancelar: () => void;
}

// Pide los datos obligatorios de Usuario que Google no entrega
const GoogleRegistroForm = ({ pendiente, onExito, onCancelar }: GoogleRegistroFormProps) => {
    const [datos, setDatos] = useState<GoogleRegistroRequest>({
        idToken: pendiente.idToken,
        nombreCompleto: pendiente.nombreCompleto,
        userName: '',
        fechaNacimiento: '',
    });
    const [hoy, setHoy] = useState(fechaLocalHoy);
    const [errores, setErrores] = useState<string[]>([]);
    const [cargando, setCargando] = useState(false);

    const actualizar = (campo: keyof GoogleRegistroRequest, valor: string) =>
        setDatos((prev) => ({ ...prev, [campo]: valor }));

    const enviar = async (e: FormEvent) => {
        e.preventDefault();
        setErrores([]);
        setCargando(true);
        try {
            onExito(await registroGoogle({
                ...datos,
                nombreCompleto: datos.nombreCompleto.trim(),
                userName: datos.userName.trim(),
            }));
        } catch (error) {
            setErrores(mensajesDeError(error));
        } finally {
            setCargando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            <p className='text-texto-suave'>
                Vas a crear tu cuenta con <span className='font-semibold text-white'>{pendiente.email}</span>.
            </p>
            <InputField label='Nombre completo' required maxLength={100} pattern='.*\S.*'
                title='El nombre no puede estar vacío' autoComplete='name'
                value={datos.nombreCompleto} onChange={(e) => actualizar('nombreCompleto', e.target.value)} />
            <InputField label='Nombre de usuario' required pattern={PATRON_USERNAME}
                title={MENSAJE_USERNAME} placeholder='juanatehortua' autoComplete='username'
                value={datos.userName} onChange={(e) => actualizar('userName', e.target.value)} />
            <InputField label='Fecha de nacimiento' type='date' required min={FECHA_MINIMA} max={hoy}
                onFocus={() => setHoy(fechaLocalHoy())}
                value={datos.fechaNacimiento} onChange={(e) => actualizar('fechaNacimiento', e.target.value)} />
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando}>{cargando ? 'Creando cuenta...' : 'Crear cuenta'}</SubmitButton>
            <button type='button' onClick={onCancelar} className='text-sm text-texto-suave hover:text-white'>
                Cancelar
            </button>
        </form>
    );
};

export default GoogleRegistroForm;
