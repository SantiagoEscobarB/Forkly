using Forkly.Api.Models.Perfil;
using Forkly.Api.Models.Restaurantes;
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

        // Perfil gastronómico (H01.09 y H01.10)
        public DbSet<OpcionPerfil> OpcionesPerfil { get; set; }
        public DbSet<PerfilGastronomico> PerfilesGastronomicos { get; set; }
        public DbSet<SeleccionPerfil> SeleccionesPerfil { get; set; }

        // Catálogo de restaurantes (FOR-61)
        public DbSet<Restaurante> Restaurantes { get; set; }
        public DbSet<RestauranteOpcion> RestauranteOpciones { get; set; }

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
                .Property(c => c.EstadoPerfil)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Entity<OpcionPerfil>(opcion =>
            {
                opcion.Property(o => o.Categoria).HasConversion<string>().HasMaxLength(20);
                opcion.Property(o => o.Codigo).HasMaxLength(60);
                opcion.Property(o => o.Nombre).HasMaxLength(60);
                opcion.HasIndex(o => o.Codigo).IsUnique();
                // Ids fijos: la migración los inserta y las selecciones los referencian
                opcion.Property(o => o.Id).ValueGeneratedNever();
                opcion.HasData(CatalogoPerfil.Opciones);
            });

            // Un perfil por comensal; si se borra el comensal se borra su perfil
            builder.Entity<PerfilGastronomico>(perfil =>
            {
                perfil.HasOne(p => p.Comensal)
                    .WithOne(c => c.PerfilGastronomico)
                    .HasForeignKey<PerfilGastronomico>(p => p.ComensalId)
                    .OnDelete(DeleteBehavior.Cascade);
                perfil.HasIndex(p => p.ComensalId).IsUnique();
            });

            builder.Entity<SeleccionPerfil>(seleccion =>
            {
                seleccion.HasKey(s => new { s.PerfilGastronomicoId, s.OpcionPerfilId });
                seleccion.HasOne(s => s.PerfilGastronomico)
                    .WithMany(p => p.Selecciones)
                    .HasForeignKey(s => s.PerfilGastronomicoId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Una opción del catálogo no se puede borrar mientras alguien la tenga elegida
                seleccion.HasOne(s => s.OpcionPerfil)
                    .WithMany()
                    .HasForeignKey(s => s.OpcionPerfilId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Reglas de las columnas de Restaurantes (FOR-61, criterio 2): ninguna queda vacía
            // ('' y solo espacios no pasan los CHECK), el teléfono va en grupos de 3, 3 y 4
            // dígitos y la fuente es una dirección https.
            builder.Entity<Restaurante>(restaurante =>
            {
                restaurante.Property(r => r.Nombre).HasMaxLength(100);
                restaurante.Property(r => r.Direccion).HasMaxLength(150);
                restaurante.Property(r => r.Barrio).HasMaxLength(60);
                restaurante.Property(r => r.Telefono).HasMaxLength(12);
                restaurante.Property(r => r.FuenteUrl).HasMaxLength(300);
                // El mismo restaurante no se carga dos veces (criterio 5)
                restaurante.HasIndex(r => new { r.Nombre, r.Direccion }).IsUnique();
                restaurante.ToTable(tabla =>
                {
                    tabla.HasCheckConstraint("CK_Restaurantes_Nombre", "[Nombre] <> ''");
                    tabla.HasCheckConstraint("CK_Restaurantes_Direccion", "[Direccion] <> ''");
                    tabla.HasCheckConstraint("CK_Restaurantes_Barrio", "[Barrio] <> ''");
                    tabla.HasCheckConstraint("CK_Restaurantes_Telefono",
                        "[Telefono] LIKE '[0-9][0-9][0-9] [0-9][0-9][0-9] [0-9][0-9][0-9][0-9]'");
                    tabla.HasCheckConstraint("CK_Restaurantes_FuenteUrl", "[FuenteUrl] LIKE 'https://_%'");
                });
                // Ids fijos: la migración los inserta y RestauranteOpciones los referencia
                restaurante.Property(r => r.Id).ValueGeneratedNever();
                restaurante.HasData(CatalogoRestaurantes.Restaurantes);
            });

            builder.Entity<RestauranteOpcion>(opcion =>
            {
                opcion.HasKey(o => new { o.RestauranteId, o.OpcionPerfilId });
                opcion.HasOne(o => o.Restaurante)
                    .WithMany(r => r.Opciones)
                    .HasForeignKey(o => o.RestauranteId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Una opción del catálogo no se puede borrar mientras un restaurante la use
                opcion.HasOne(o => o.OpcionPerfil)
                    .WithMany()
                    .HasForeignKey(o => o.OpcionPerfilId)
                    .OnDelete(DeleteBehavior.Restrict);
                opcion.HasData(CatalogoRestaurantes.Opciones);
            });
        }
    }
}
