using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
        [Route("api/TipoLibro/Filter/{textoBusqueda?}")]
        public List<Shared.TipoLibro> Filter(string textoBusqueda)
        {
            List<Shared.TipoLibro> lista = new List<Shared.TipoLibro>();
            //using (BDBibliotecaContext db = new BDBibliotecaContext())
            //{
                lista = _context.TipoLibro
                    .Where(tipoLibro => tipoLibro.Bhabilitado == 1 && (textoBusqueda == null || tipoLibro.Nombretipolibro.Contains(textoBusqueda)) )
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
    }
}