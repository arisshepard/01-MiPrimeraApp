using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [Route("api/Sexo/Get")]
        public List<Shared.Sexo> GetSexo()
        {
            List<Shared.Sexo> sexos = new List<Shared.Sexo>();

            sexos = _context.Sexo
                .Where(sexo => sexo.Bhabilitado == 1)
                .Select(sexo => new Shared.Sexo
                {
                    ID = sexo.Iidsexo,
                    Nombre = sexo.Nombre
                })
                .ToList();

            return sexos;
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

        [HttpGet]
        [Route("api/Autor/getById/{ID}")]
        public async Task<Shared.Autor> GetByIdAsync(int ID)
        {
            Autor autorDb = await _context.Autor.Where(autor => autor.Iidautor == ID).FirstOrDefaultAsync();
            Shared.Autor autor = null;

            if (autorDb != null)
            {
                autor = new Shared.Autor
                {
                    ID = autorDb.Iidautor,
                    Descripcion = autorDb.Descripcion,
                    IdPais = autorDb.Iidpais.ToString(),
                    IdSexo = autorDb.Iidsexo.ToString(),
                    Nombre = autorDb.Nombre,
                    PrimerApellido = autorDb.Appaterno,
                    SegundoApellido = autorDb.Apmaterno
                };
            }

            return autor;
        }

        [HttpPost]
        [Route("api/Autor/save")]
        public async Task<int> GuardarAsync(Shared.Autor autor)
        {
            int respuesta;
            try
            {
                if (autor.ID == 0)
                {
                    Autor autorDb = new Autor
                    {
                        Apmaterno = autor.SegundoApellido,
                        Appaterno = autor.PrimerApellido,
                        Bhabilitado = 1,
                        Descripcion = autor.Descripcion,
                        Iidpais = int.Parse(autor.IdPais),
                        Iidsexo = int.Parse(autor.IdSexo),
                        Nombre = autor.Nombre
                    };

                    await _context.Autor.AddAsync(autorDb);
                }
                else
                {
                    Autor autorDb = await _context.Autor.Where(autorDb => autorDb.Iidautor == autor.ID).FirstAsync();

                    autorDb.Apmaterno = autor.SegundoApellido;
                    autorDb.Appaterno = autor.PrimerApellido;
                    autorDb.Bhabilitado = 1;
                    autorDb.Descripcion = autor.Descripcion;
                    autorDb.Iidpais = int.Parse(autor.IdPais);
                    autorDb.Iidsexo = int.Parse(autor.IdSexo);
                    autorDb.Nombre = autor.Nombre;
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
    }
}