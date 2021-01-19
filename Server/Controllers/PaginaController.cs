using _01_MiPrimeraApp.Server.Models;
using _01_MiPrimeraApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class PaginaController : Controller
    {
        private readonly BDBibliotecaContext _context;

        public PaginaController(BDBibliotecaContext context)
        {
            _context = context;
        }

        #region "Métodos GET"

        [HttpGet]
        [Route("api/Pagina/GetAll")]
        public async Task<List<Shared.Pagina>> GetAllAsync()
        {
            List<Shared.Pagina> paginas = new List<Shared.Pagina>();

            paginas = await _context.Pagina
                .Where(paginaDb => paginaDb.Bhabilitado == 1)
                .Select(paginaDb => new Shared.Pagina()
                {
                    Accion = paginaDb.Accion,
                    ID = paginaDb.Iidpagina,
                    Mensaje = paginaDb.Mensaje,
                    NombreVisible = paginaDb.Bvisible == 1 ? "Si" : "No"
                })
                .ToListAsync();

            return paginas;
        }

        [HttpGet]
        [Route("api/Pagina/Filter/{text?}")]
        public async Task<List<Shared.Pagina>> FilterAsync(string text)
        {
            List<Shared.Pagina> paginas = new List<Shared.Pagina>();

            paginas = await _context.Pagina
                .Where(paginaDb => paginaDb.Bhabilitado == 1 &&
                (string.IsNullOrEmpty(text) || paginaDb.Mensaje.Contains(text)))
                .Select(paginaDb => new Shared.Pagina()
                {
                    Accion = paginaDb.Accion,
                    ID = paginaDb.Iidpagina,
                    Mensaje = paginaDb.Mensaje,
                    NombreVisible = paginaDb.Bvisible == 1 ? "Si" : "No"
                })
                .ToListAsync();

            return paginas;
        }

        [HttpGet]
        [Route("api/Pagina/DeleteById/{ID}")]
        public async Task<int> DeleteByIDAsync(string ID)
        {
            int respuesta = 0;

            try
            {
                Models.Pagina paginaDb = await _context.Pagina
                    .Where(pagina => pagina.Iidpagina.ToString() == ID)
                    .FirstOrDefaultAsync();

                if (paginaDb != null)
                {
                    paginaDb.Bhabilitado = 0;
                    await _context.SaveChangesAsync();

                    respuesta = 1;
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Pagina/GetById/{ID}")]
        public async Task<Shared.Pagina> GetByIDAsync(string ID)
        {
            Shared.Pagina pagina = await _context.Pagina
                .Where(paginaDb => paginaDb.Iidpagina.ToString() == ID)
                .Select(paginaDb => new Shared.Pagina()
                {
                    Accion = paginaDb.Accion,
                    ID = paginaDb.Iidpagina,
                    Mensaje = paginaDb.Mensaje,
                    Visible = Convert.ToBoolean(paginaDb.Bvisible),
                    NombreVisible = paginaDb.Bvisible == 1 ? "Si" : "No"
                })
                .FirstOrDefaultAsync();

            return pagina;
        }

        #endregion "Métodos GET"

        #region "Métodos POST"

        [HttpPost]
        [Route("api/Pagina/Create")]
        public async Task<int> CreateAsync([FromBody] Shared.Pagina pagina)
        {
            int respuesta = 0;

            try
            {
                Models.Pagina paginaDb = new Models.Pagina()
                {
                    Accion = pagina.Accion,
                    Bhabilitado = 1,
                    Bvisible = Convert.ToInt32(pagina.Visible),
                    Mensaje = pagina.Mensaje
                };
                await _context.Pagina.AddAsync(paginaDb);
                await _context.SaveChangesAsync();

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
        [Route("api/Pagina/UpdateById/{ID}")]
        public async Task<int> UpdateByIdASync(int ID, [FromBody] Shared.Pagina pagina)
        {
            int respuesta = 0;

            try
            {
                Models.Pagina paginaDb = await _context.Pagina
                    .Where(pagina => pagina.Iidpagina == ID)
                    .FirstOrDefaultAsync();

                if (paginaDb != null)
                {
                    paginaDb.Accion = pagina.Accion;
                    paginaDb.Bvisible = Convert.ToInt32(pagina.Visible);
                    paginaDb.Mensaje = pagina.Mensaje;
                }

                await _context.SaveChangesAsync();

                respuesta = 1;
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }

        #endregion
    }
}