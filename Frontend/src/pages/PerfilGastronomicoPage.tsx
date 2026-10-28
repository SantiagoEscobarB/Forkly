import { useEffect, useState } from 'react';
import { Link } from 'react-router';
import AvisoDietas from '../components/perfil/AvisoDietas';
import GrupoOpciones from '../components/perfil/GrupoOpciones';
import ErrorMessage from '../components/ui/ErrorMessage';
import Logo from '../components/ui/Logo';
import SubmitButton from '../components/ui/SubmitButton';
import { useSesion } from '../hooks/useSesion';
import { RUTAS } from '../routes/rutas';
import { mensajesDeError } from '../services/api';
import { guardarPerfil, obtenerOpciones, obtenerPerfil } from '../services/perfilService';
import { ORDEN_CATEGORIAS, type OpcionPerfil } from '../types/perfil';
import { alternar, hayVariasDietas } from '../utils/perfil';

// Ajustes del perfil gastronómico: editar preferencias (H01.09, criterio 5) y ver y cambiar
// las restricciones declaradas (H01.10, criterio 3). Al guardar se recalcula el vector.
const PerfilGastronomicoPage = () => {
    const { sesion, actualizarUsuario } = useSesion();
    const [opciones, setOpciones] = useState<OpcionPerfil[]>([]);
    const [seleccion, setSeleccion] = useState<string[]>([]);
    const [cargando, setCargando] = useState(true);
    const [guardando, setGuardando] = useState(false);
    const [errores, setErrores] = useState<string[]>([]);
    const [aviso, setAviso] = useState('');

    useEffect(() => {
        let cancelado = false;
        Promise.all([obtenerOpciones(), obtenerPerfil()])
            .then(([catalogo, perfil]) => {
                if (cancelado) return;
                setOpciones(catalogo);
                setSeleccion(perfil.seleccionadas.map((opcion) => opcion.codigo));
                setCargando(false);
            })
            .catch((error: unknown) => {
                if (cancelado) return;
                setErrores(mensajesDeError(error));
                setCargando(false);
            });
        return () => {
            cancelado = true;
        };
    }, []);

    const alternarOpcion = (codigo: string) => {
        setAviso('');
        setSeleccion((actual) => alternar(actual, codigo));
    };

    const guardar = async () => {
        setErrores([]);
        setAviso('');
        setGuardando(true);
        try {
            const perfil = await guardarPerfil({ opciones: seleccion });
            if (sesion) actualizarUsuario({ ...sesion.usuario, estadoPerfil: perfil.estado });
            setAviso('Guardamos tus cambios.');
        } catch (error) {
            setErrores(mensajesDeError(error));
        } finally {
            setGuardando(false);
        }
    };

    const variasDietas = hayVariasDietas(seleccion, opciones);

    return (
        <main className='mx-auto flex min-h-screen w-full max-w-3xl flex-col gap-8 px-4 py-12'>
            <div className='flex items-center justify-between gap-4'>
                <Logo />
                <Link to={RUTAS.inicio} className='text-sm text-texto-suave hover:text-white'>Volver al inicio</Link>
            </div>
            <div>
                <h1 className='font-display text-3xl font-bold'>Mi perfil gastronómico</h1>
                <p className='mt-1 text-texto-suave'>Cambia tus gustos y restricciones cuando quieras.</p>
            </div>

            {cargando ? (
                <p className='text-texto-suave'>Cargando tu perfil...</p>
            ) : (
                <div className='flex flex-col gap-8 rounded-3xl border border-borde bg-panel/40 p-6'>
                    {ORDEN_CATEGORIAS.map((categoria) => (
                        <GrupoOpciones key={categoria} categoria={categoria} opciones={opciones}
                            seleccion={seleccion} onAlternar={alternarOpcion} deshabilitado={guardando} />
                    ))}
                    <AvisoDietas visible={variasDietas} />
                </div>
            )}

            <ErrorMessage errores={errores} />
            {aviso && <p role='status' className='text-sm text-green-400'>{aviso}</p>}
            <SubmitButton type='button' onClick={guardar} disabled={cargando || guardando || variasDietas}>
                {guardando ? 'Guardando...' : 'Guardar cambios'}
            </SubmitButton>
        </main>
    );
};

export default PerfilGastronomicoPage;
