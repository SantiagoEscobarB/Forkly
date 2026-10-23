import { useRef, useState, type ClipboardEvent, type KeyboardEvent } from 'react';
import { LARGO_CODIGO } from '../../types/verificacion';

interface CodigoInputProps {
    // Recibe solo los dígitos escritos; el código está completo cuando tiene LARGO_CODIGO
    onChange: (codigo: string) => void;
    deshabilitado?: boolean;
}

// Una casilla por dígito. Avanza sola al escribir, retrocede con Backspace
// y reparte el código si se pega o el navegador lo autocompleta.
const CodigoInput = ({ onChange, deshabilitado = false }: CodigoInputProps) => {
    const [digitos, setDigitos] = useState<string[]>(Array(LARGO_CODIGO).fill(''));
    const casillas = useRef<(HTMLInputElement | null)[]>([]);

    const enfocar = (indice: number) =>
        casillas.current[Math.min(Math.max(indice, 0), LARGO_CODIGO - 1)]?.focus();

    const actualizar = (nuevos: string[]) => {
        setDigitos(nuevos);
        onChange(nuevos.join(''));
    };

    const escribir = (indice: number, texto: string) => {
        const soloDigitos = texto.replace(/\D/g, '');
        const nuevos = [...digitos];

        if (!soloDigitos) {
            nuevos[indice] = '';
            actualizar(nuevos);
            return;
        }

        const aEscribir = soloDigitos.slice(0, LARGO_CODIGO - indice).split('');
        aEscribir.forEach((digito, j) => { nuevos[indice + j] = digito; });
        actualizar(nuevos);
        enfocar(indice + aEscribir.length);
    };

    const alPresionar = (indice: number, e: KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Backspace' && !digitos[indice]) {
            e.preventDefault();
            const nuevos = [...digitos];
            nuevos[indice - 1] = '';
            if (indice > 0) actualizar(nuevos);
            enfocar(indice - 1);
        }
        if (e.key === 'ArrowLeft') enfocar(indice - 1);
        if (e.key === 'ArrowRight') enfocar(indice + 1);
    };

    const alPegar = (indice: number, e: ClipboardEvent<HTMLInputElement>) => {
        e.preventDefault();
        escribir(indice, e.clipboardData.getData('text'));
    };

    return (
        <div role='group' aria-label='Código de verificación' className='flex justify-between gap-2'>
            {digitos.map((digito, indice) => (
                <input key={indice} ref={(el) => { casillas.current[indice] = el; }}
                    value={digito} disabled={deshabilitado}
                    inputMode='numeric' autoComplete={indice === 0 ? 'one-time-code' : 'off'}
                    aria-label={`Dígito ${indice + 1}`}
                    onChange={(e) => escribir(indice, e.target.value)}
                    onKeyDown={(e) => alPresionar(indice, e)}
                    onPaste={(e) => alPegar(indice, e)}
                    // Al enfocar se selecciona el dígito para que escribir lo reemplace
                    onFocus={(e) => e.target.select()}
                    className='h-14 w-12 rounded-2xl border border-borde bg-panel text-center font-display text-2xl
                               font-bold text-white outline-none transition-colors focus:border-primary
                               disabled:opacity-60 sm:h-16 sm:w-14' />
            ))}
        </div>
    );
};

export default CodigoInput;
