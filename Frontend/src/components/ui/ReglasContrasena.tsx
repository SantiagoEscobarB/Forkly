import { reglasContrasena } from '../../utils/validaciones';

interface ReglasContrasenaProps {
    // Para enlazarla con el campo mediante aria-describedby
    id: string;
    contrasena: string;
}

// Lista de reglas que se marcan en verde a medida que la contraseña las cumple
const ReglasContrasena = ({ id, contrasena }: ReglasContrasenaProps) => {
    return (
        <ul id={id} className='grid grid-cols-2 gap-x-4 gap-y-1 px-2 text-sm'>
            {reglasContrasena.map((regla) => {
                const cumple = regla.cumple(contrasena);
                return (
                    <li key={regla.texto} className={cumple ? 'text-green-400' : 'text-texto-suave'}>
                        <span aria-hidden='true'>{cumple ? '✓' : '•'}</span> {regla.texto}
                        {cumple && <span className='sr-only'> (cumplido)</span>}
                    </li>
                );
            })}
        </ul>
    );
};

export default ReglasContrasena;
