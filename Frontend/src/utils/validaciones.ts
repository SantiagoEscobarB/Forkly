// Validaciones compartidas por los formularios de registro (con contraseña y con Google)

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
