using _01_MiPrimeraApp.Server.Models;
using _01_MiPrimeraApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class TipoUsuarioController : Controller
    {
        private readonly BDBibliotecaContext _context;
        private readonly IEntityMappingService<TipoUsuario, Shared.TipoUsuario> _mappingService;

        public TipoUsuarioController(BDBibliotecaContext context,
            IEntityMappingService<TipoUsuario, Shared.TipoUsuario> mappingService)
        {
            _context = context;
            _mappingService = mappingService;
        }

        #region "Métodos GET"

        [HttpGet]
        [Route("api/TipoUsuario/DeleteById/{ID}")]
        public async Task<int> DeleteByIDAsync(string ID)
        {
            int respuesta = 0;

            try
            {
                TipoUsuario tipoUsuarioDb = await _context.TipoUsuario
                    .Where(tipoUsuarioDb => tipoUsuarioDb.Iidtipousuario.ToString() == ID)
                    .FirstOrDefaultAsync();

                if (tipoUsuarioDb != null)
                {
                    tipoUsuarioDb.Bhabilitado = 0;
                    await _context.SaveChangesAsync();

                    respuesta = 1;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/TipoUsuario/Filter/{text?}")]
        public async Task<List<Shared.TipoUsuario>> FilterAsync(string text)
        {
            List<TipoUsuario> tiposUsuarioDb = await _context.TipoUsuario
                .Where(tipoUsuarioDb => tipoUsuarioDb.Bhabilitado == 1 &&
                (string.IsNullOrEmpty(text) || tipoUsuarioDb.Nombre.Contains(text)))
                .ToListAsync();

            return _mappingService.Map(tiposUsuarioDb).ToList();
        }

        [HttpGet]
        [Route("api/TipoUsuario/GetAll")]
        public async Task<List<Shared.TipoUsuario>> GetAllAsync()
        {
            List<Shared.TipoUsuario> tiposUsuario = new List<Shared.TipoUsuario>();

            List<TipoUsuario> tiposUsuarioDb = await _context.TipoUsuario
                .Where(tiposUsuario => tiposUsuario.Bhabilitado == 1)
                .ToListAsync();

            tiposUsuario = _mappingService.Map(tiposUsuarioDb).ToList();

            return tiposUsuario;
        }

        [HttpGet]
        [Route("api/TipoUsuario/GetById/{ID}")]
        public async Task<Shared.TipoUsuario> GetByIDAsync(string ID)
        {
            TipoUsuario tipoUsuarioDb = await _context.TipoUsuario
                .Where(tipoUsuarioDb => tipoUsuarioDb.Iidtipousuario.ToString() == ID)
                .FirstOrDefaultAsync();

            List<PaginaTipoUsuario> paginas = await _context.PaginaTipoUsuario
                .Where(pagina => pagina.Iidtipousuario == tipoUsuarioDb.Iidtipousuario &&
                pagina.Bhabilitado == 1).ToListAsync();

            tipoUsuarioDb.PaginaTipoUsuario = paginas;

            return _mappingService.Map(tipoUsuarioDb);
        }

        #endregion "Métodos GET"

        #region "Métodos POST"

        [HttpPost]
        [Route("api/TipoUsuario/Create")]
        public async Task<int> CreateAsync([FromBody] Shared.TipoUsuario tipoUsuario)
        {
            int respuesta = 0;

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                TipoUsuario tipoUsuarioDb = new TipoUsuario()
                {
                    Bhabilitado = 1,
                    Descripcion = tipoUsuario.Descripcion,
                    Nombre = tipoUsuario.Nombre
                };
                await _context.TipoUsuario.AddAsync(tipoUsuarioDb);
                await _context.SaveChangesAsync();

                int ID = tipoUsuarioDb.Iidtipousuario;
                foreach (var numero in tipoUsuario.PaginasID)
                {
                    PaginaTipoUsuario paginaTipoUsuario = new PaginaTipoUsuario()
                    {
                        Iidpagina = numero,
                        Iidtipousuario = ID,
                        Bhabilitado = 1
                    };

                    await _context.PaginaTipoUsuario.AddAsync(paginaTipoUsuario);
                    // await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                respuesta = 1;
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }

        #endregion "Métodos POST"

        #region "Métodos PUT"

        [HttpPut]
        [Route("api/TipoUsuario/UpdateById/{ID}")]
        public async Task<int> UpdateByIdASync(int ID, [FromBody] Shared.TipoUsuario tipoUsuario)
        {
            int respuesta = 0;

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                TipoUsuario tipoUsuarioDb = await _context.TipoUsuario
                    .Where(tipoUsuarioDb => tipoUsuarioDb.Iidtipousuario == ID)
                    .Include(tipoUsuarioDb => tipoUsuarioDb.PaginaTipoUsuario)
                    .FirstOrDefaultAsync();

                if (tipoUsuarioDb != null)
                {
                    tipoUsuarioDb.Descripcion = tipoUsuario.Descripcion;
                    tipoUsuarioDb.Nombre = tipoUsuario.Nombre;
                }

                // Paginas que antes estaban
                tipoUsuarioDb.PaginaTipoUsuario
                    .Where(pagina => pagina.Iidtipousuario == tipoUsuarioDb.Iidtipousuario)
                    .Select(pagina =>
                    {
                        pagina.Bhabilitado = tipoUsuario.PaginasID.Contains((int)pagina.Iidpagina) ? 1 : 0;
                        return pagina;
                    }).ToList();

                // Paginas nuevas
                foreach (var numero in tipoUsuario.PaginasID)
                {
                    if (!_context.PaginaTipoUsuario.Any(pagina => pagina.Iidtipousuario == ID && pagina.Iidpagina == numero))
                    {
                        _context.PaginaTipoUsuario.Add(new PaginaTipoUsuario()
                        {
                            Iidpagina = numero,
                            Iidtipousuario = ID,
                            Bhabilitado = 1
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                respuesta = 1;
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }

        #endregion "Métodos PUT"
    }
}