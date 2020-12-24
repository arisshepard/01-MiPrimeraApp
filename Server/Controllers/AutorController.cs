using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class AutorController : Controller
    {
        private readonly BDBibliotecaContext _context;

        public AutorController(BDBibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/Autor/Get")]
        public List<Shared.Autor> Get()
        {
            List<Shared.Autor> autores = new List<Shared.Autor>();

            autores = (from autor in _context.Autor
                       join pais in _context.Pais
                       on autor.Iidpais equals pais.Iidpais
                       join sexo in _context.Sexo
                       on autor.Iidsexo equals sexo.Iidsexo
                       where autor.Bhabilitado == 1
                       select new Shared.Autor
                       {
                           ID = autor.Iidautor,
                           Nombre = $"{autor.Nombre} {autor.Appaterno} {autor.Apmaterno}",
                           Sexo = sexo.Nombre,
                           Pais = pais.Nombre,
                           Descripcion = autor.Descripcion
                       }).ToList();

            return autores;
        }

        [HttpGet]
        [Route("api/Autor/Filter/{data?}")]
        public List<Shared.Autor> Filter(string data)
        {
            List<Shared.Autor> autores = new List<Shared.Autor>();

            autores = (from autor in _context.Autor
                       join pais in _context.Pais
                       on autor.Iidpais equals pais.Iidpais
                       join sexo in _context.Sexo
                       on autor.Iidsexo equals sexo.Iidsexo
                       where (autor.Bhabilitado == 1 && (data == null || autor.Iidpais.ToString() == data))
                       select new Shared.Autor
                       {
                           ID = autor.Iidautor,
                           Nombre = $"{autor.Nombre} {autor.Appaterno} {autor.Apmaterno}",
                           Sexo = sexo.Nombre,
                           Pais = pais.Nombre,
                           Descripcion = autor.Descripcion
                       }).ToList();

            return autores;
        }

        [HttpGet]
        [Route("api/Pais/Get")]
        public List<Shared.Pais> GetPais()
        {
            List<Shared.Pais> paises = new List<Shared.Pais>();

            paises = _context.Pais.Where(pais => pais.Bhabilitado == 1).Select(pais => new Shared.Pais { ID = pais.Iidpais, Nombre = pais.Nombre }).ToList();

            return paises;
        }

        [HttpGet]
        [Route("api/Autor/delete/{ID}")]
        public int EliminarAutor(string ID)
        {
            int respuesta = 0;

            try
            {
                Autor autor = _context.Autor.Where(autor => autor.Iidautor.ToString() == ID).FirstOrDefault();
                autor.Bhabilitado = 0;
                _context.SaveChanges();
                respuesta = 1;
            }
            catch (Exception ex)
            {

                throw;
            }

            return respuesta;
        }

    }
}
