using Microsoft.AspNetCore.Identity;

namespace Forkly.Api.Service.Auth
{
    // Traduce los mensajes de Identity que el frontend muestra al registrarse
    public class ErroresIdentityEs : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) =>
            new() { Code = nameof(DuplicateUserName), Description = $"El nombre de usuario '{userName}' ya está en uso." };

        public override IdentityError DuplicateEmail(string email) =>
            new() { Code = nameof(DuplicateEmail), Description = $"El correo '{email}' ya está registrado." };

        public override IdentityError InvalidUserName(string? userName) =>
            new() { Code = nameof(InvalidUserName), Description = "El nombre de usuario solo puede tener letras, números, puntos, guiones y guion bajo." };

        public override IdentityError InvalidEmail(string? email) =>
            new() { Code = nameof(InvalidEmail), Description = "El correo no es válido." };

        public override IdentityError PasswordTooShort(int length) =>
            new() { Code = nameof(PasswordTooShort), Description = $"La contraseña debe tener mínimo {length} caracteres." };

        public override IdentityError PasswordRequiresUpper() =>
            new() { Code = nameof(PasswordRequiresUpper), Description = "La contraseña debe tener una mayúscula." };

        public override IdentityError PasswordRequiresLower() =>
            new() { Code = nameof(PasswordRequiresLower), Description = "La contraseña debe tener una minúscula." };

        public override IdentityError PasswordRequiresDigit() =>
            new() { Code = nameof(PasswordRequiresDigit), Description = "La contraseña debe tener un número." };

        public override IdentityError PasswordRequiresNonAlphanumeric() =>
            new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La contraseña debe tener un símbolo." };
    }
}
