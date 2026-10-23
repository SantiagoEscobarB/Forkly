// Backend/Models/DTOs/Verificacion/VerificarCorreoDTO.cs
export interface VerificarCorreoRequest {
    codigo: string; // 6 dígitos
}

export const LARGO_CODIGO = 6;
