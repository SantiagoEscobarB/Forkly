import { useState, type FormEvent } from 'react';
import { ApiError, mensajesDeError } from '../../services/api';
import { restablecerContrasena, STATUS_ENLACE_INVALIDO } from '../../services/recuperacionService';
import type { EnlaceRecuperacion } from '../../types/recuperacion';
import { cumpleReglasContrasena } from '../../utils/validaciones';
import ErrorMessage from '../ui/ErrorMessage';
import InputField from '../ui/InputField';
import ReglasContrasena from '../ui/ReglasContrasena';
import SubmitButton from '../ui/SubmitButton';

interface RestablecerContrasenaFormProps {
    enlace: EnlaceRecuperacion;
    onRestablecida: () => void;
    // El enlace venció o se usó mientras el usuario escribía
    onEnlaceInvalido: () => void;
}

const RestablecerContrasenaForm = ({ enlace, onRestablecida, onEnlaceInvalido }: RestablecerContrasenaFormProps) => {
    const [contrasena, setContrasena] = useState('');
    const [confirmacion, setConfirmacion] = useState('');
    const [errores, setErrores] = useState<string[]>([]);
    const [cargando, setCargando] = useState(false);

    const contrasenaValida = cumpleReglasContrasena(contrasena);
    const coinciden = contrasena === confirmacion;

    const enviar = async (e: FormEvent) => {
        e.preventDefault();
        if (!contrasenaValida || !coinciden) return;
        setErrores([]);
        setCargando(true);
        try {
            await restablecerContrasena({ ...enlace, nuevaContrasena: contrasena });
            onRestablecida();
        } catch (error) {
            if (error instanceof ApiError && error.status === STATUS_ENLACE_INVALIDO) onEnlaceInvalido();
            else setErrores(mensajesDeError(error));
        } finally {
            setCargando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            <div className='flex flex-col gap-3'>
                <InputField label='Contraseña nueva' type='password' required placeholder='••••••••'
                    autoComplete='new-password' aria-describedby='reglas-contrasena-nueva'
                    value={contrasena} onChange={(e) => setContrasena(e.target.value)} />
                <ReglasContrasena id='reglas-contrasena-nueva' contrasena={contrasena} />
            </div>
            <div className='flex flex-col gap-2'>
                <InputField label='Confirma la contraseña' type='password' required placeholder='••••••••'
                    autoComplete='new-password' aria-invalid={confirmacion !== '' && !coinciden}
                    value={confirmacion} onChange={(e) => setConfirmacion(e.target.value)} />
                {confirmacion !== '' && !coinciden && (
                    <p className='px-2 text-sm text-red-400'>Las contraseñas no coinciden.</p>
                )}
            </div>
            <ErrorMessage errores={errores} />
            <SubmitButton disabled={cargando || !contrasenaValida || !coinciden}>
                {cargando ? 'Guardando...' : 'Guardar contraseña'}
            </SubmitButton>
        </form>
    );
};

export default RestablecerContrasenaForm;
