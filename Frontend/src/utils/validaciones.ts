// Validaciones compartidas por los formularios de registro (con contraseña y con Google)
// y por el de restablecer la contraseña

// Mismos caracteres que options.User.AllowedUserNameCharacters en Backend/Program.cs.
// El guion va escapado porque el navegador evalúa `pattern` con el flag `v`.
export const PATRON_USERNAME = '[a-zA-Z0-9._\\-]+';
export const MENSAJE_USERNAME = 'Solo letras, números, puntos, guiones y guion bajo';

// Fecha local en "yyyy-MM-dd" (toISOString usa UTC y en Colombia daría mañana desde las 7 p. m.)
export const fechaLocalHoy = () => {
    const d = new Date();
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
};

export const FECHA_MINIMA = '1900-01-01';

// Mismas reglas que el PasswordValidator por defecto de Identity
// (solo cuenta letras y números ASCII, igual que el backend).
// Las usan el registro y el restablecimiento de la contraseña.
export const reglasContrasena = [
    { texto: 'Mínimo 6 caracteres', cumple: (p: string) => p.length >= 6 },
    { texto: 'Una mayúscula', cumple: (p: string) => /[A-Z]/.test(p) },
    { texto: 'Una minúscula', cumple: (p: string) => /[a-z]/.test(p) },
    { texto: 'Un número', cumple: (p: string) => /[0-9]/.test(p) },
    { texto: 'Un símbolo', cumple: (p: string) => /[^a-zA-Z0-9]/.test(p) },
];

export const cumpleReglasContrasena = (contrasena: string) =>
    reglasContrasena.every((regla) => regla.cumple(contrasena));
