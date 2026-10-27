using Forkly.Api.DAO;
using Forkly.Api.Interface;
using Forkly.Api.Models.DTOs.Perfil;
using Forkly.Api.Models.Enums;
using Forkly.Api.Models.Perfil;
using Forkly.Api.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Forkly.Api.Service.Perfil
{
    // Configuración del perfil gastronómico (H01.09) y restricciones alimentarias (H01.10).
    // Vector de preferencias: cada tipo de cocina, ambiente y rango de precio elegido pesa
    // 1 / (cantidad de opciones elegidas en su categoría); lo no elegido pesa 0.
    public class PerfilService : IPerfilService
    {
        private static readonly CategoriaOpcion[] CategoriasDelVector =
            [CategoriaOpcion.COCINA, CategoriaOpcion.AMBIENTE, CategoriaOpcion.PRECIO];

        private readonly ApplicationDbContext _db;

        public PerfilService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<OpcionPerfilDTO>> ObtenerOpciones()
        {
            // Se ordena en memoria: en la base la categoría está guardada como texto
            var opciones = await _db.OpcionesPerfil.AsNoTracking().ToListAsync();
            return Ordenar(opciones).Select(OpcionPerfilDTO.DesdeEntidad).ToList();
        }

        public async Task<PerfilDTO> ObtenerPerfil(string usuarioId) =>
            await CrearDTO(await BuscarComensal(usuarioId));

        // Cada vez que el usuario cambia algo en el asistente se guarda lo elegido y el paso,
        // así puede interrumpir y retomar donde iba (H01.09, criterio 4)
        public async Task<PerfilDTO> GuardarAvance(string usuarioId, AvancePerfilDTO dto)
        {
            var comensal = await BuscarComensal(usuarioId);
            if (comensal.EstadoPerfil is EstadoPerfil.COMPLETO or EstadoPerfil.OMITIDO)
                throw new PerfilYaConfiguradoException();

            var opciones = await ValidarOpciones(dto.Opciones);
            var perfil = ObtenerOCrearPerfil(comensal);
            ReemplazarSelecciones(perfil, opciones, calcularPesos: false);
            perfil.PasoActual = dto.Paso;
            comensal.EstadoPerfil = EstadoPerfil.EN_PROGRESO;

            await _db.SaveChangesAsync();
            return await CrearDTO(comensal);
        }

        // Confirmar el asistente o editar desde los ajustes: guarda y (re)calcula el vector
        // (H01.09, criterios 2 y 5; H01.10, criterio 1)
        public async Task<PerfilDTO> Guardar(string usuarioId, GuardarPerfilDTO dto)
        {
            var comensal = await BuscarComensal(usuarioId);
            var opciones = await ValidarOpciones(dto.Opciones);
            var perfil = ObtenerOCrearPerfil(comensal);
            ReemplazarSelecciones(perfil, opciones, calcularPesos: true);
            perfil.PasoActual = 1;
            comensal.EstadoPerfil = EstadoPerfil.COMPLETO;

            await _db.SaveChangesAsync();
            return await CrearDTO(comensal);
        }

        // Perfil vacío; no se vuelve a pedir la configuración (H01.09, criterio 3)
        public async Task<PerfilDTO> Omitir(string usuarioId)
        {
            var comensal = await BuscarComensal(usuarioId);
            if (comensal.EstadoPerfil == EstadoPerfil.COMPLETO) throw new PerfilYaConfiguradoException();

            var perfil = ObtenerOCrearPerfil(comensal);
            ReemplazarSelecciones(perfil, new List<OpcionPerfil>(), calcularPesos: false);
            perfil.PasoActual = 1;
            comensal.EstadoPerfil = EstadoPerfil.OMITIDO;

            await _db.SaveChangesAsync();
            return await CrearDTO(comensal);
        }

        // El id sale del JWT, así que el usuario existe salvo que lo hayan borrado
        private async Task<Comensal> BuscarComensal(string usuarioId) =>
            await _db.Comensales
                .Include(c => c.PerfilGastronomico!)
                    .ThenInclude(p => p.Selecciones)
                    .ThenInclude(s => s.OpcionPerfil)
                .FirstOrDefaultAsync(c => c.Id == usuarioId)
            ?? throw new KeyNotFoundException("El usuario no existe.");

        // Devuelve las opciones del catálogo que corresponden a los códigos recibidos.
        // El catálogo es pequeño, así que se valida en memoria.
        private async Task<List<OpcionPerfil>> ValidarOpciones(IEnumerable<string> codigos)
        {
            var pedidos = codigos
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct()
                .ToList();

            var catalogo = await _db.OpcionesPerfil.ToListAsync();
            var opciones = catalogo.Where(o => pedidos.Contains(o.Codigo)).ToList();

            if (opciones.Count != pedidos.Count)
                throw new PerfilInvalidoException("Algunas opciones no existen. Recarga la página e intenta de nuevo.");

            // H01.10, criterio 2: solo un tipo de dieta
            if (opciones.Count(o => o.Categoria == CategoriaOpcion.DIETA) > 1)
                throw new PerfilInvalidoException(
                    "Elegiste más de un tipo de dieta. Elige solo una: vegetariana, vegana o pescetariana.");

            return Ordenar(opciones).ToList();
        }

        private PerfilGastronomico ObtenerOCrearPerfil(Comensal comensal)
        {
            if (comensal.PerfilGastronomico != null) return comensal.PerfilGastronomico;

            var perfil = new PerfilGastronomico { ComensalId = comensal.Id, Comensal = comensal };
            _db.PerfilesGastronomicos.Add(perfil);
            comensal.PerfilGastronomico = perfil;
            return perfil;
        }

        // Quita lo que ya no está elegido, agrega lo nuevo y actualiza los pesos.
        // No se borra todo y se vuelve a insertar para no repetir claves en el mismo guardado.
        private void ReemplazarSelecciones(PerfilGastronomico perfil, List<OpcionPerfil> opciones, bool calcularPesos)
        {
            var elegidas = opciones.Select(o => o.Id).ToHashSet();

            foreach (var quitada in perfil.Selecciones.Where(s => !elegidas.Contains(s.OpcionPerfilId)).ToList())
            {
                perfil.Selecciones.Remove(quitada);
                _db.SeleccionesPerfil.Remove(quitada);
            }

            foreach (var opcion in opciones)
            {
                var seleccion = perfil.Selecciones.FirstOrDefault(s => s.OpcionPerfilId == opcion.Id);
                if (seleccion == null)
                {
                    seleccion = new SeleccionPerfil { OpcionPerfilId = opcion.Id, OpcionPerfil = opcion };
                    perfil.Selecciones.Add(seleccion);
                }
                seleccion.Peso = calcularPesos ? PesoEnElVector(opcion, opciones) : null;
            }

            perfil.ActualizadoEn = DateTime.UtcNow;
        }

        // Mismo peso para cada opción elegida dentro de su categoría (H01.09, criterio 2).
        // Las restricciones no forman parte del vector.
        private static double? PesoEnElVector(OpcionPerfil opcion, List<OpcionPerfil> elegidas)
        {
            if (!CategoriasDelVector.Contains(opcion.Categoria)) return null;
            return 1.0 / elegidas.Count(o => o.Categoria == opcion.Categoria);
        }

        private async Task<PerfilDTO> CrearDTO(Comensal comensal)
        {
            var perfil = comensal.PerfilGastronomico;
            var seleccionadas = perfil?.Selecciones.ToList() ?? new List<SeleccionPerfil>();

            var dto = new PerfilDTO
            {
                // null = PENDIENTE, que es el primer valor del enum
                Estado = comensal.EstadoPerfil.GetValueOrDefault(),
                PasoActual = perfil?.PasoActual ?? 1,
                ActualizadoEn = perfil?.ActualizadoEn,
                Seleccionadas = Ordenar(seleccionadas.Select(s => s.OpcionPerfil))
                    .Select(OpcionPerfilDTO.DesdeEntidad)
                    .ToList()
            };

            // El vector se calcula al confirmar el perfil; incluye todas las opciones de
            // preferencia del catálogo, con 0 las que no se eligieron
            if (dto.Estado == EstadoPerfil.COMPLETO)
            {
                var pesos = seleccionadas
                    .Where(s => s.Peso.HasValue)
                    .ToDictionary(s => s.OpcionPerfilId, s => s.Peso!.Value);
                var catalogo = await _db.OpcionesPerfil.AsNoTracking().ToListAsync();

                dto.Vector = Ordenar(catalogo.Where(o => CategoriasDelVector.Contains(o.Categoria)))
                    .ToDictionary(o => o.Codigo, o => pesos.GetValueOrDefault(o.Id));
            }

            return dto;
        }

        private static IEnumerable<OpcionPerfil> Ordenar(IEnumerable<OpcionPerfil> opciones) =>
            opciones.OrderBy(o => o.Categoria).ThenBy(o => o.Orden);
    }
}
