import { useEffect, useState, type FormEvent } from 'react';
import { mensajesDeError } from '../../services/api';
import { marcarCodigoEnviado, segundosDesdeCodigoEnviado } from '../../services/sesionStorage';
import { reenviarCodigo, verificarCorreo } from '../../services/verificacionService';
import type { Comensal } from '../../types/usuario';
import { LARGO_CODIGO } from '../../types/verificacion';
import ErrorMessage from '../ui/ErrorMessage';
import SubmitButton from '../ui/SubmitButton';
import CodigoInput from './CodigoInput';

// Tiempo mínimo entre reenvíos (el backend además limita a 3 cada 10 minutos)
const ESPERA_REENVIO = 60;

const esperaRestante = () => Math.max(0, ESPERA_REENVIO - segundosDesdeCodigoEnviado());

interface VerificacionFormProps {
    onVerificado: (usuario: Comensal) => void;
}

const VerificacionForm = ({ onVerificado }: VerificacionFormProps) => {
    const [codigo, setCodigo] = useState('');
    const [errores, setErrores] = useState<string[]>([]);
    const [aviso, setAviso] = useState('');
    const [verificando, setVerificando] = useState(false);
    const [reenviando, setReenviando] = useState(false);
    const [espera, setEspera] = useState(esperaRestante);

    useEffect(() => {
        if (espera <= 0) return;
        const temporizador = setTimeout(() => setEspera(esperaRestante()), 1000);
        return () => clearTimeout(temporizador);
    }, [espera]);

    const enviar = async (e: FormEvent) => {
        e.preventDefault();
        setErrores([]);
        setAviso('');
        setVerificando(true);
        try {
            onVerificado(await verificarCorreo({ codigo }));
        } catch (error) {
            setErrores(mensajesDeError(error));
        } finally {
            setVerificando(false);
        }
    };

    const reenviar = async () => {
        setErrores([]);
        setAviso('');
        setReenviando(true);
        try {
            await reenviarCodigo();
            marcarCodigoEnviado();
            setEspera(ESPERA_REENVIO);
            setAviso('Te enviamos un código nuevo. Revisa también la carpeta de spam.');
        } catch (error) {
            setErrores(mensajesDeError(error));
        } finally {
            setReenviando(false);
        }
    };

    return (
        <form onSubmit={enviar} className='flex flex-col gap-6'>
            <CodigoInput onChange={setCodigo} deshabilitado={verificando} />
            <ErrorMessage errores={errores} />
            {aviso && <p role='status' className='text-sm text-green-400'>{aviso}</p>}
            <SubmitButton disabled={verificando || codigo.length !== LARGO_CODIGO}>
                {verificando ? 'Verificando...' : 'Verificar correo'}
            </SubmitButton>
            <p className='text-center text-sm text-texto-suave'>
                ¿No te llegó el código?{' '}
                <button type='button' onClick={reenviar} disabled={reenviando || espera > 0}
                    className='font-semibold text-primary hover:underline disabled:text-texto-suave disabled:no-underline'>
                    {espera > 0 ? `Reenviar en ${espera} s` : reenviando ? 'Enviando...' : 'Reenviar código'}
                </button>
            </p>
        </form>
    );
};

export default VerificacionForm;
