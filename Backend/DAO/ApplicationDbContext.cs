using Forkly.Api.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forkly.Api.DAO
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Usuario es abstracta: EF guarda a todos los usuarios en AspNetUsers (TPH)
        // y necesita conocer los tipos concretos que heredan de ella.
        public DbSet<Comensal> Comensales { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(usuario =>
            {
                usuario.Property(u => u.NombreCompleto).HasMaxLength(100);
                // Enums como texto para que reordenarlos no cambie el significado de los datos
                usuario.Property(u => u.Estado).HasConversion<string>().HasMaxLength(20);
            });

            builder.Entity<Comensal>()
                .Property(c => c.Nivel)
                .HasConversion<string>()
                .HasMaxLength(20);
        }
    }
}
