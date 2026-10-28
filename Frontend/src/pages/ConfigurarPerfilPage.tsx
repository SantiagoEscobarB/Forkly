import { useEffect, useRef, useState } from 'react';
import AuthLayout from '../components/layout/AuthLayout';
import AvisoDietas from '../components/perfil/AvisoDietas';
import GrupoOpciones from '../components/perfil/GrupoOpciones';
import ProgresoPasos from '../components/perfil/ProgresoPasos';
import ErrorMessage from '../components/ui/ErrorMessage';
import SubmitButton from '../components/ui/SubmitButton';
import { useSesion } from '../hooks/useSesion';
import { mensajesDeError } from '../services/api';
import { guardarAvance, guardarPerfil, obtenerOpciones, obtenerPerfil, omitirPerfil } from '../services/perfilService';
import { PASOS_PERFIL, type OpcionPerfil, type Perfil } from '../types/perfil';
import type { EstadoPerfil } from '../types/usuario';
import { alternar, hayVariasDietas } from '../utils/perfil';

const TOTAL_PASOS = PASOS_PERFIL.length;

// H01.09: asistente de 4 pasos que aparece después de verificar el correo.
// Cada cambio se guarda en el backend, así que si se interrumpe se retoma donde iba.
// Al confirmar u omitir cambia el estado del perfil y RutaProtegida lleva al inicio.
const ConfigurarPerfilPage = () => {
    const { sesion, actualizarUsuario } = useSesion();
    const [opciones, setOpciones] = useState<OpcionPerfil[]>([]);
    const [seleccion, setSeleccion] = useState<string[]>([]);
    const [paso, setPaso] = useState(1);
    const [estadoInicial, setEstadoInicial] = useState<EstadoPerfil | null>(null);
    const [cargando, setCargando] = useState(true);
    const [guardando, setGuardando] = useState(false);
    const [errores, setErrores] = useState<string[]>([]);
    // Los avances se envían en orden, uno detrás de otro, para que el último siempre gane
    const cola = useRef<Promise<void>>(Promise.resolve());

    useEffect(() => {
        let cancelado = false;
        Promise.all([obtenerOpciones(), obtenerPerfil()])
            .then(([catalogo, perfil]) => {
                if (cancelado) return;
                setOpciones(catalogo);
                setSeleccion(perfil.seleccionadas.map((opcion) => opcion.codigo));
                setPaso(Math.min(Math.max(perfil.pasoActual, 1), TOTAL_PASOS));
                setEstadoInicial(perfil.estado);
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

    const guardarEnCola = (nuevoPaso: number, nuevaSeleccion: string[]) => {
        cola.current = cola.current
            .then(() => guardarAvance({ paso: nuevoPaso, opciones: nuevaSeleccion }))
            .then(() => setErrores([]))
            .catch((error: unknown) => setErrores(mensajesDeError(error)));
    };

    const alternarOpcion = (codigo: string) => {
        const nueva = alternar(seleccion, codigo);
        setSeleccion(nueva);
        // Con dos dietas el backend rechazaría el avance; se espera a que el usuario corrija
        if (!hayVariasDietas(nueva, opciones)) guardarEnCola(paso, nueva);
    };

    const irAPaso = (nuevoPaso: number) => {
        setPaso(nuevoPaso);
        guardarEnCola(nuevoPaso, seleccion);
    };

    // El nuevo estado (COMPLETO u OMITIDO) hace que RutaProtegida lleve al inicio
    const terminar = (perfil: Perfil) => {
        if (sesion) actualizarUsuario({ ...sesion.usuario, estadoPerfil: perfil.estado });
    };

    const ejecutar = async (accion: () => Promise<Perfil>) => {
        setErrores([]);
        setGuardando(true);
        try {
            await cola.current;
            terminar(await accion());
        } catch (error) {
            setErrores(mensajesDeError(error));
            setGuardando(false);
        }
    };

    const ultimoPaso = paso === TOTAL_PASOS;
    const variasDietas = hayVariasDietas(seleccion, opciones);

    return (
        <AuthLayout titulo='Configura tu perfil gastronómico' subtitulo={`Paso ${paso} de ${TOTAL_PASOS}`}>
            {estadoInicial === 'PENDIENTE' && paso === 1 && (
                <p role='status' className='rounded-2xl border border-green-400/30 bg-green-400/10 px-4 py-3 text-sm text-green-300'>
                    Tu cuenta está verificada. Cuéntanos qué te gusta para recomendarte mejor desde el primer día.
                </p>
            )}
            <ProgresoPasos paso={paso} total={TOTAL_PASOS} />

            {cargando ? (
                <p className='text-texto-suave'>Cargando opciones...</p>
            ) : (
                <div className='flex flex-col gap-6'>
                    {PASOS_PERFIL[paso - 1].map((categoria) => (
                        <GrupoOpciones key={categoria} categoria={categoria} opciones={opciones}
                            seleccion={seleccion} onAlternar={alternarOpcion} deshabilitado={guardando} />
                    ))}
                    {ultimoPaso && (
                        <p className='text-sm text-texto-suave'>Si no tienes restricciones, deja todo sin marcar.</p>
                    )}
                    <AvisoDietas visible={ultimoPaso && variasDietas} />
                </div>
            )}

            <ErrorMessage errores={errores} />

            <div className='flex gap-3'>
                {paso > 1 && (
                    <button type='button' onClick={() => irAPaso(paso - 1)} disabled={guardando}
                        className='rounded-full border border-borde bg-panel px-6 py-4 font-semibold transition-colors
                                   hover:border-primary disabled:opacity-60'>
                        Atrás
                    </button>
                )}
                {ultimoPaso ? (
                    <SubmitButton type='button' disabled={cargando || guardando || variasDietas}
                        onClick={() => ejecutar(() => guardarPerfil({ opciones: seleccion }))}>
                        {guardando ? 'Guardando...' : 'Guardar perfil'}
                    </SubmitButton>
                ) : (
                    <SubmitButton type='button' disabled={cargando || guardando} onClick={() => irAPaso(paso + 1)}>
                        Siguiente
                    </SubmitButton>
                )}
            </div>
            <button type='button' onClick={() => ejecutar(omitirPerfil)} disabled={guardando}
                className='text-sm text-texto-suave hover:text-white'>
                Omitir por ahora
            </button>
        </AuthLayout>
    );
};

export default ConfigurarPerfilPage;
