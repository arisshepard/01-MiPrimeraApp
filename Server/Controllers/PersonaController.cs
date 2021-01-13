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
    public class PersonaController : Controller
    {
        #region "Propiedades"

        private readonly BDBibliotecaContext _context;

        #endregion "Propiedades"

        #region "Constructor"

        public PersonaController(BDBibliotecaContext context)
        {
            _context = context;
        }

        #endregion "Constructor"

        #region "Get Methods"

        [HttpGet]
        [Route("api/Persona/GetAll")]
        public async Task<List<Shared.Persona>> GetAllAsync()
        {
            List<Shared.Persona> personas = new List<Shared.Persona>();

            personas = await _context.Persona
                .Where(persona => persona.Bhabilitado == 1)
                .Select(persona => new Shared.Persona
                {
                    Email = persona.Correo,
                    FechaFormato = persona.Fechanacimiento == null ? string.Empty : string.Format("{0:dd/MM/yyyy}", persona.Fechanacimiento),
                    ID = persona.Iidpersona,
                    Nombre = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}"
                }).ToListAsync();

            return personas;
        }

        [HttpGet]
        [Route("api/Persona/GetById/{id}")]
        public async Task<ActionResult<Shared.Persona>> GetByIdAsync(int id)
        {
            Shared.Persona persona = await _context.Persona
                .Where(persona => persona.Iidpersona == id)
                .Select(persona => new Shared.Persona
                {
                    Email = persona.Correo,
                    Fecha = (DateTime)persona.Fechanacimiento,
                    NombreSimple = persona.Nombre,
                    PrimerApellido = persona.Appaterno,
                    SegundoApellido = persona.Apmaterno,
                    Telefono = persona.Telefono,
                    ID = persona.Iidpersona
                })
                .FirstOrDefaultAsync();

            return persona;
        }

        [HttpGet]
        [Route("api/Persona/FilterByName/{name?}")]
        public async Task<List<Shared.Persona>> FilterByNameAsync(string name)
        {
            List<Shared.Persona> personas = new List<Shared.Persona>();

            personas = await _context.Persona
                    .Where(persona => persona.Bhabilitado == 1 &&
                          (!string.IsNullOrEmpty(name) && (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(name) ||
                          string.IsNullOrEmpty(name))
                          )
                //.Where(persona => persona.Bhabilitado == 1 && persona.Nombre.Contains(name))
                .Select(persona => new Shared.Persona
                {
                    Email = persona.Correo,
                    FechaFormato = persona.Fechanacimiento == null ? string.Empty : string.Format("{0:dd/MM/yyyy}", persona.Fechanacimiento),
                    ID = persona.Iidpersona,
                    Nombre = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}"
                }).ToListAsync();

            return personas;
        }

        [HttpGet]
        [Route("api/Persona/Delete/{ID}")]
        public async Task<int> DeleteById(int ID)
        {
            int respuesta;

            try
            {
                Persona personaDb = await _context.Persona.Where(persona => persona.Iidpersona == ID).FirstOrDefaultAsync();

                if (personaDb != null)
                {
                    personaDb.Bhabilitado = 0;
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

        #endregion "Get Methods"

        #region "POST Methods"

        [HttpPost]
        [Route("api/Persona/Save")]
        public async Task<ActionResult<int>> Guardar([FromBody] Shared.Persona persona)
        {
            int respuesta = 0;
            try
            {
                if (persona.ID == 0)
                {
                    Persona personaBD = new Persona
                    {
                        Apmaterno = persona.SegundoApellido,
                        Appaterno = persona.PrimerApellido,
                        Correo = persona.Email,
                        Nombre = persona.NombreSimple,
                        Telefono = persona.Telefono,
                        Fechanacimiento = persona.Fecha,
                        Bhabilitado = 1,
                        Btieneusuario = 0
                    };
                    _context.Persona.Add(personaBD);
                }
                else
                {
                    Persona personaBD = await _context.Persona
                        .Where(personaDb => personaDb.Iidpersona == persona.ID)
                        .FirstOrDefaultAsync();

                    personaBD.Apmaterno = persona.SegundoApellido;
                    personaBD.Appaterno = persona.PrimerApellido;
                    personaBD.Correo = persona.Email;
                    personaBD.Nombre = persona.NombreSimple;
                    personaBD.Telefono = persona.Telefono;
                    personaBD.Fechanacimiento = persona.Fecha;
                }

                await _context.SaveChangesAsync();

                respuesta = 1;
            }
            catch (Exception ex)
            {
            }
            return respuesta;
        }

        #endregion "POST Methods"
    }
}