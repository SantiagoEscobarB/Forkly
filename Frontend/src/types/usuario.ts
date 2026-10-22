// Espejo de Backend/Models/DTOs/ComensalDTO.cs. ASP.NET Core serializa las
// propiedades en camelCase y los enums como texto (JsonStringEnumConverter).

// Backend/Models/Enums/EstadoCuenta.cs
export type EstadoCuenta = 'VERIFICADA' | 'NO_VERIFICADA' | 'BLOQUEADA';

// Backend/Models/Enums/NivelComensal.cs
export type NivelComensal = 'NORMAL' | 'PREMIUM';

// Backend/Models/Users/Usuario.cs. Los campos que vienen de IdentityUser
// conservan su nombre; PasswordHash nunca sale del backend.
export interface Usuario {
    id: string;
    nombreCompleto: string;
    email: string;
    userName: string;
    fechaNacimiento: string; // DateOnly -> "yyyy-MM-dd"
    estado: EstadoCuenta;
    twoFactorEnabled: boolean;
}

// Backend/Models/Users/Comensal.cs
export interface Comensal extends Usuario {
    perfilPublico: boolean;
    nivel: NivelComensal;
}
