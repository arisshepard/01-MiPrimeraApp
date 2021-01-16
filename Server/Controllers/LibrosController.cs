using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class LibrosController : Controller
    {
        private readonly BDBibliotecaContext _context;

        public LibrosController(BDBibliotecaContext context)
        {
            _context = context;
        }

        #region "Métodos GET"

        [HttpGet]
        [Route("api/libros/filter/{text?}")]
        public async Task<List<Shared.Libro>> FilterAsync(string text)
        {
            List<Shared.Libro> libros = new List<Shared.Libro>();

            libros = await (from libro in _context.Libro
                            join autor in _context.Autor on libro.Iidautor equals autor.Iidautor
                            where libro.Bhabilitado == 1 &&
                            (string.IsNullOrEmpty(text) || libro.Titulo.Contains(text))
                            select new Shared.Libro
                            {
                                Autor = $"{autor.Appaterno} {autor.Apmaterno}, {autor.Nombre}",
                                Fotocaratula = libro.Fotocaratula,
                                ID = libro.Iidlibro,
                                IDAutor = autor.Iidautor.ToString(),
                                Libropdf = libro.Libropdf,
                                Numpaginas = libro.Numpaginas,
                                Resumen = libro.Resumen,
                                Stock = libro.Stock,
                                Titulo = libro.Titulo
                            }).ToListAsync();

            return libros;
        }

        [HttpGet]
        [Route("api/libros/getAll")]
        public async Task<List<Shared.Libro>> GetAllAsync()
        {
            List<Shared.Libro> libros = new List<Shared.Libro>();

            libros = await (from libro in _context.Libro
                            join autor in _context.Autor on libro.Iidautor equals autor.Iidautor
                            where libro.Bhabilitado == 1
                            select new Shared.Libro
                            {
                                Autor = $"{autor.Appaterno} {autor.Apmaterno}, {autor.Nombre}",
                                Fotocaratula = libro.Fotocaratula,
                                ID = libro.Iidlibro,
                                IDAutor = autor.Iidautor.ToString(),
                                Libropdf = libro.Libropdf,
                                Numpaginas = libro.Numpaginas,
                                Resumen = libro.Resumen,
                                Stock = libro.Stock,
                                Titulo = libro.Titulo
                            }).ToListAsync();

            return libros;
        }

        [HttpGet]
        [Route("api/libros/getById/{ID}")]
        public async Task<Shared.Libro> GetByIDAsync(int ID)
        {
            Shared.Libro libro = await (from libroDb in _context.Libro
                                        join autor in _context.Autor on libroDb.Iidautor equals autor.Iidautor
                                        where libroDb.Iidlibro == ID
                                        select new Shared.Libro
                                        {
                                            Autor = $"{autor.Appaterno} {autor.Apmaterno}, {autor.Nombre}",
                                            Fotocaratula = libroDb.Fotocaratula,
                                            ID = libroDb.Iidlibro,
                                            IDAutor = autor.Iidautor.ToString(),
                                            Libropdf = libroDb.Libropdf,
                                            Numpaginas = libroDb.Numpaginas,
                                            Resumen = libroDb.Resumen,
                                            Stock = libroDb.Stock,
                                            Titulo = libroDb.Titulo
                                        }).FirstOrDefaultAsync();

            return libro;
        }

        #endregion "Métodos GET"

        #region "Métodos POST"

        [HttpPost]
        [Route("api/libros/create")]
        public async Task<int> CreateAsync([FromBody] Shared.Libro libro)
        {
            int respuesta = 0;
            try
            {
                Libro libroDb = new Libro
                {
                    Bhabilitado = 1,
                    Fotocaratula = libro.Fotocaratula,
                    Iidautor = int.Parse(libro.IDAutor),
                    Libropdf = libro.Libropdf,
                    Numpaginas = libro.Numpaginas,
                    Resumen = libro.Resumen,
                    Stock = libro.Stock,
                    Titulo = libro.Titulo
                };

                await _context.Libro.AddAsync(libroDb);
                await _context.SaveChangesAsync();

                respuesta = 1;
            }
            catch (System.Exception)
            {
                throw;
            }

            return respuesta;
        }

        #endregion "Métodos POST"

        #region "Métodos PUT"

        [HttpPut]
        [Route("api/libros/delete/{ID}")]
        public async Task<int> DeleteById(int ID)
        {
            int respuesta = 0;

            try
            {
                Libro libroDb = await _context.Libro.Where(libro => libro.Iidlibro == ID).FirstOrDefaultAsync();

                if (libroDb != null)
                {
                    libroDb.Bhabilitado = 0;
                    await _context.SaveChangesAsync();
                }

                respuesta = 1;
            }
            catch (System.Exception)
            {
                throw;
            }

            return respuesta;
        }

        [HttpPut]
        [Route("api/libros/updateById/{ID}")]
        public async Task<int> UpdateByIDAsync(int ID, [FromBody] Shared.Libro libro)
        {
            int respuesta = 0;
            try
            {
                Libro libroDb = await _context.Libro
                    .Where(libro => libro.Iidlibro == ID)
                    .FirstOrDefaultAsync();

                libroDb.Fotocaratula = libro.Fotocaratula;
                libroDb.Iidautor = int.Parse(libro.IDAutor);
                libroDb.Libropdf = libro.Libropdf;
                libroDb.Numpaginas = libro.Numpaginas;
                libroDb.Resumen = libro.Resumen;
                libroDb.Stock = libro.Stock;
                libroDb.Titulo = libro.Titulo;

                await _context.SaveChangesAsync();

                respuesta = 1;
            }
            catch (System.Exception)
            {
                throw;
            }

            return respuesta;
        }

        #endregion "Métodos PUT"
    }
}