using _01_MiPrimeraApp.Server.Clases;
using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        [Route("api/Persona/GetID/{IDUsuario}")]
        public async Task<int> GetIDAsync(int IDUsuario)
        {
            int IDPersona = 0;
            try
            {
                IDPersona = await _context.Usuario
                    .Where(usuario => usuario.Iidusuario == IDUsuario)
                    .Select(usuario => (int)usuario.Iidpersona)
                    .FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return IDPersona;
        }

        [HttpGet]
        [Route("api/Persona/GetById/{id}")]
        public async Task<ActionResult<Shared.Persona>> GetByIdAsync(int id)
        {
            Shared.Persona persona = await (from personaDb
                                            in _context.Persona
                                            join usuarioDb in _context.Usuario
                                            on personaDb.Iidpersona equals usuarioDb.Iidpersona
                                            where personaDb.Iidpersona == id
                                            select new Shared.Persona()
                                            {
                                                Email = personaDb.Correo,
                                                Fecha = (DateTime)personaDb.Fechanacimiento,
                                                NombreSimple = personaDb.Nombre,
                                                PrimerApellido = personaDb.Appaterno,
                                                SegundoApellido = personaDb.Apmaterno,
                                                Telefono = personaDb.Telefono,
                                                ID = personaDb.Iidpersona,
                                                NombreUsuario = usuarioDb.Nombreusuario,
                                                IDTipoUsuario = usuarioDb.Iidtipousuario.ToString()
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
                _context.Database.BeginTransaction();
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
                        Btieneusuario = 1
                    };
                    _context.Persona.Add(personaBD);
                    await _context.SaveChangesAsync();

                    // Encriptado
                    string clave = persona.PasswordUsuario;
                    SHA256Managed sha = new SHA256Managed();
                    byte[] buffer = Encoding.Default.GetBytes(clave);
                    byte[] passwordCifrada = sha.ComputeHash(buffer);

                    // Usuario asociado
                    Usuario usuarioDb = new Usuario()
                    {
                        Bhabilitado = 1,
                        Contra = BitConverter.ToString(passwordCifrada).Replace("-", ""),
                        Iidpersona = personaBD.Iidpersona,
                        Iidtipousuario = int.Parse(persona.IDTipoUsuario),
                        Nombreusuario = persona.NombreUsuario,
                        Token = null
                    };
                    _context.Usuario.Add(usuarioDb);
                    // await _context.SaveChangesAsync();
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

                    // Usuario
                    Usuario usuarioDb = await _context.Usuario
                        .Where(usuario => usuario.Iidpersona == personaBD.Iidpersona)
                        .FirstOrDefaultAsync();

                    usuarioDb.Nombreusuario = persona.NombreUsuario;
                    usuarioDb.Iidtipousuario = int.Parse(persona.IDTipoUsuario);

                }

                await _context.SaveChangesAsync();

                respuesta = 1;

                _context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
            }
            return respuesta;
        }

        [HttpPost]
        [Route("api/Persona/Nuevo")]
        public async Task<ActionResult<int>> NuevoAsync([FromBody] Shared.UsuarioRegister usuarioRegister)
        {
            int respuesta = 0;
            try
            {
                _context.Database.BeginTransaction();

                Persona personaBD = new Persona
                {
                    Apmaterno = usuarioRegister.SegundoApellido,
                    Appaterno = usuarioRegister.PrimerApellido,
                    Correo = usuarioRegister.Email,
                    Nombre = usuarioRegister.NombreSimple,
                    Telefono = usuarioRegister.Telefono,
                    Fechanacimiento = usuarioRegister.Fecha,
                    Bhabilitado = 1,
                    Btieneusuario = 1
                };
                _context.Persona.Add(personaBD);
                await _context.SaveChangesAsync();

                // Encriptado
                string clave = usuarioRegister.PasswordUsuario;
                SHA256Managed sha = new SHA256Managed();
                byte[] buffer = Encoding.Default.GetBytes(clave);
                byte[] passwordCifrada = sha.ComputeHash(buffer);

                // Usuario asociado
                Usuario usuarioDb = new Usuario()
                {
                    Bhabilitado = 1,
                    Contra = BitConverter.ToString(passwordCifrada).Replace("-", ""),
                    Iidpersona = personaBD.Iidpersona,
                    Iidtipousuario = 5,
                    Nombreusuario = usuarioRegister.NombreSimple
                };

                // generar token
                string token = string.Empty;
                for (int i = 0; i < 20; i++)
                {
                    Random r = new Random();
                    int n = r.Next(0, 9);
                    token += n.ToString();
                }

                // Generic.EnviarCorreo(personaBD.Correo, "Activar cuenta", $"Haz click <a href='https://localhost:4434:/token/{token}'>aquí</a>");

                usuarioDb.Token = token;

                _context.Usuario.Add(usuarioDb);
                // await _context.SaveChangesAsync();

                await _context.SaveChangesAsync();

                respuesta = 1;

                _context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
            }
            return respuesta;
        }

        #endregion "POST Methods"
    }
}