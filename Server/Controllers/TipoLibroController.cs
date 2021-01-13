using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Controllers
{
    public class TipoLibroController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly BDBibliotecaContext _context;

        public TipoLibroController(BDBibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/TipoLibro/Get")]
        public List<Shared.TipoLibro> Get()
        {
            List<Shared.TipoLibro> lista = new List<Shared.TipoLibro>();
            //using (BDBibliotecaContext db = new BDBibliotecaContext())
            //{
            lista = _context.TipoLibro
                .Where(tipoLibro => tipoLibro.Bhabilitado == 1)
                .Select(tipoLibro => new Shared.TipoLibro
                {
                    ID = tipoLibro.Iidtipolibro,
                    Nombre = tipoLibro.Nombretipolibro,
                    Descripcion = tipoLibro.Descripcion
                })
                .ToList();
            //}

            return lista;
        }

        [HttpGet]
        [Route("api/TipoLibro/Delete/{ID}")]
        public int Delete(string ID)
        {
            int respuesta = 0;

            try
            {
                TipoLibro tipoLibro = _context.TipoLibro.Where(tipoLibro => tipoLibro.Iidtipolibro.ToString() == ID).FirstOrDefault();
                tipoLibro.Bhabilitado = 0;
                _context.SaveChanges();
                respuesta = 1;
            }
            catch (Exception ex)
            {
                throw;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/TipoLibro/Filter/{textoBusqueda?}")]
        public List<Shared.TipoLibro> Filter(string textoBusqueda)
        {
            List<Shared.TipoLibro> lista = new List<Shared.TipoLibro>();
            //using (BDBibliotecaContext db = new BDBibliotecaContext())
            //{
            lista = _context.TipoLibro
                .Where(tipoLibro => tipoLibro.Bhabilitado == 1 && (textoBusqueda == null || tipoLibro.Nombretipolibro.Contains(textoBusqueda)))
                .Select(tipoLibro => new Shared.TipoLibro
                {
                    ID = tipoLibro.Iidtipolibro,
                    Nombre = tipoLibro.Nombretipolibro,
                    Descripcion = tipoLibro.Descripcion
                })
                .ToList();
            //}

            return lista;
        }

        [HttpGet]
        [Route("api/TipoLibro/GetById/{id}")]
        public async Task<ActionResult<Shared.TipoLibro>> GetByIdAsync(int id)
        {
            Shared.TipoLibro tipoLibro = await _context.TipoLibro
                .Where(tipoLibro => tipoLibro.Iidtipolibro == id)
                .Select(tipoLibro => new Shared.TipoLibro
                {
                    Descripcion = tipoLibro.Descripcion,
                    ID = tipoLibro.Iidtipolibro,
                    Nombre = tipoLibro.Nombretipolibro
                })
                .FirstOrDefaultAsync();

            return tipoLibro;
        }

        [HttpPost]
        [Route("api/TipoLibro/Save")]
        public async Task<ActionResult<int>> Guardar([FromBody] Shared.TipoLibro tipoLibro)
        {
            int respuesta = 0;
            try
            {
                if (tipoLibro.ID == 0)
                {
                    TipoLibro tipoLibroBD = new TipoLibro
                    {
                        Descripcion = tipoLibro.Descripcion,
                        Nombretipolibro = tipoLibro.Nombre,
                        Bhabilitado = 1
                    };

                    _context.TipoLibro.Add(tipoLibroBD);
                    await _context.SaveChangesAsync();

                    respuesta = 1;
                }
                else
                {
                    TipoLibro tipoLibroBD = await _context.TipoLibro
                        .Where(tipoLibroDb => tipoLibroDb.Iidtipolibro == tipoLibro.ID)
                        .FirstOrDefaultAsync();

                    tipoLibroBD.Nombretipolibro = tipoLibro.Nombre;
                    tipoLibroBD.Descripcion = tipoLibro.Descripcion;

                    await _context.SaveChangesAsync();

                    respuesta = 1;

                }
            }
            catch (Exception ex)
            {
            }
            return respuesta;
        }
    }
}